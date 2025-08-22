import { useState, useEffect } from 'react';
import './App.css';
import dgiiLogo from '../src/assets/dgii-logo.png';

function App() {
  const [contributors, setContributors] = useState([]);
  const [selectedContributor, setSelectedContributor] = useState(null);
  const [fiscalReceipts, setFiscalReceipts] = useState([]);
  const [totalItbis, setTotalItbis] = useState(null);

  useEffect(() => {
    // Fetch contributors
    fetch('https://localhost:7113/api/contribuyentes')
      .then(response => response.json())
      .then(data => setContributors(data))
      .catch(error => console.error('Error fetching contributors:', error));
  }, []);

  const handleContributorClick = (rncCedula) => {
    setSelectedContributor(rncCedula);
    // Fetch fiscal receipts
    fetch(`https://localhost:7113/api/contribuyentes/${rncCedula}/comprobantes`)
      .then(response => response.json())
      .then(data => setFiscalReceipts(data))
      .catch(error => console.error('Error fetching fiscal receipts:', error));
    // Fetch total ITBIS
    fetch(`https://localhost:7113/api/contribuyentes/${rncCedula}/total-itbis`)
      .then(response => response.json())
      .then(data => setTotalItbis(data))
      .catch(error => console.error('Error fetching total ITBIS:', error));
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
