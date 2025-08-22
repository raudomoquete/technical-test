# DGII Backend

Este proyecto es una aplicación backend para la gestión de contribuyentes y comprobantes fiscales. Está construido utilizando ASP.NET Core y sigue una arquitectura basada en capas.

## Estructura del Proyecto

- **DGII.API**: Contiene los controladores de la API que manejan las solicitudes HTTP y devuelven respuestas JSON.
- **DGII.Application**: Incluye la lógica de negocio y los manejadores de consultas y comandos.
- **DGII.Domain**: Define las entidades y los objetos de valor del dominio.
- **DGII.Infrastructure**: Proporciona la implementación de acceso a datos y la configuración de la base de datos.

## Ejecución de Pruebas

Las pruebas están ubicadas en el directorio `DGII.Tests` y están escritas utilizando xUnit. Para ejecutar las pruebas, sigue estos pasos:

1. Abre una terminal en el directorio raíz del proyecto.
2. Ejecuta el siguiente comando para ejecutar todas las pruebas:
   ```bash
   dotnet test
   ```

Este comando compilará el proyecto de pruebas y ejecutará todas las pruebas definidas en `DGII.Tests`.

## Ejecución del Frontend

El frontend está desarrollado con React y Vite. Para ejecutarlo, sigue estos pasos:

1. Abre una terminal en el directorio `DGII.Frontend/my-app`.
2. Instala las dependencias del proyecto:
   ```bash
   npm install
   ```
3. Inicia el servidor de desarrollo:
   ```bash
   npm run dev
   ```
4. Abre tu navegador y visita la URL que aparece en la terminal, generalmente `http://localhost:3000`.

## Navegación en el Frontend

- **Listado de Contribuyentes**: Al abrir la aplicación, verás una lista de contribuyentes con detalles como nombre, tipo, RNC/Cédula y estatus.
- **Interacción**: Haz clic en cualquier contribuyente para desplegar más detalles sobre sus comprobantes fiscales y el total de ITBIS.
- **Detalles de Comprobantes**: Al hacer clic, se mostrarán los detalles de los comprobantes fiscales, incluyendo NCF, monto e ITBIS.