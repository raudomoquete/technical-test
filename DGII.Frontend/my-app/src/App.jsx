import { useState, useEffect } from 'react';
import './App.css';
import dgiiLogo from '../src/assets/dgii-logo.png';

function App() {
  const [contributors, setContributors] = useState([]);
  const [selectedContributor, setSelectedContributor] = useState(null);
  const [fiscalReceipts, setFiscalReceipts] = useState([]);
  const [totalItbis, setTotalItbis] = useState(null);

  useEffect(() => {
    // Fetch contributors and their fiscal receipts in one go if possible
    fetch('https://localhost:7113/api/contribuyentes')
      .then(response => {
        if (!response.ok) {
          throw new Error('Network response was not ok');
        }
        return response.json();
      })
      .then(data => {
        if (Array.isArray(data)) {
          setContributors(data);
        } else {
          console.error('Invalid data format');
        }
      })
      .catch(error => console.error('Error fetching contributors:', error));
  }, []);

  const handleContributorClick = (rncCedula) => {
    setSelectedContributor(rncCedula);
    // Fetch fiscal receipts and total ITBIS
    Promise.all([
      fetch(`https://localhost:7113/api/contribuyentes/${rncCedula}/comprobantes`),
      fetch(`https://localhost:7113/api/contribuyentes/${rncCedula}/total-itbis`)
    ])
      .then(async ([receiptsResponse, itbisResponse]) => {
        if (!receiptsResponse.ok || !itbisResponse.ok) {
          throw new Error('Network response was not ok');
        }
        const receiptsData = await receiptsResponse.json();
        const itbisData = await itbisResponse.json();
        if (Array.isArray(receiptsData)) {
          setFiscalReceipts(receiptsData);
        } else {
          console.error('Invalid receipts data format');
        }
        setTotalItbis(itbisData);
      })
      .catch(error => console.error('Error fetching data:', error));
  };

  return (
    <div className="App">
      <header className="App-header">
        <img src={dgiiLogo} className="App-logo" alt="DGII logo" />
        <h1>DGII Contribuyentes</h1>
      </header>
      <main>
        <section>
          <h2>Contribuyentes</h2>
          <ul>
            {contributors.map(contributor => (
              <li key={contributor.rncCedula} onClick={() => handleContributorClick(contributor.rncCedula)}>
                <p>Nombre: {contributor.nombre}</p>
                <p>Tipo: {contributor.tipo}</p>
                <p>RNC/Cédula: {contributor.rncCedula}</p>
                <p>Estatus: {contributor.estatus}</p>
              </li>
            ))}
          </ul>
        </section>
        {selectedContributor && (
          <section>
            <h2>Comprobantes Fiscales</h2>
            <ul>
              {fiscalReceipts.map((receipt, index) => (
                <li key={`${receipt.NCF}-${index}`}>
                  <p>NCF: {receipt.NCF}</p>
                  <p>Monto: {receipt.monto}</p>
                  <p>ITBIS: {receipt.itbis18}</p>
                </li>
              ))}
            </ul>
            <h3>Total ITBIS: {totalItbis ? totalItbis.totalItbis : 'No disponible'}</h3>
          </section>
        )}
      </main>
    </div>
  );
}

export default App;
