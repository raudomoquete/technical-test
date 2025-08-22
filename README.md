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