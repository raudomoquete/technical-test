using DGII.Domain.Entities;
using DGII.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace DGII.Infrastructure.Data;

public static class DgiiDbContextSeed
{
    public static async Task SeedAsync(DgiiDbContext context)
    {
        // Verificar si ya hay datos
        if (await context.Contribuyentes.AnyAsync())
        {
            return; // Ya hay datos, no hacer nada
        }

        // Crear contribuyentes según el requerimiento
        var contribuyentes = new List<Contribuyente>
        {
            new Contribuyente
            {
                Id = Guid.NewGuid(),
                rncCedula = "98754321012",
                nombre = "JUAN PEREZ",
                tipo = TipoContribuyente.PersonaFisica.ToDisplayString(),
                estatus = EstatusContribuyente.Activo.ToDisplayString()
            },
            new Contribuyente
            {
                Id = Guid.NewGuid(),
                rncCedula = "123456789",
                nombre = "FARMACIA TU SALUD",
                tipo = TipoContribuyente.PersonaJuridica.ToDisplayString(),
                estatus = EstatusContribuyente.Inactivo.ToDisplayString()
            }
        };

        await context.Contribuyentes.AddRangeAsync(contribuyentes);
        await context.SaveChangesAsync();

        // Obtener los IDs de los contribuyentes para crear los comprobantes fiscales
        var juanPerez = await context.Contribuyentes.FirstAsync(c => c.rncCedula == "98754321012");

        // Crear comprobantes fiscales según el requerimiento
        var comprobantesFiscales = new List<ComprobanteFiscal>
        {
            new ComprobanteFiscal
            {
                Id = Guid.NewGuid(),
                ContribuyenteId = juanPerez.Id,
                NCF = "E310000000001",
                monto = 200.00m,
                itbis18 = 36.00m
            },
            new ComprobanteFiscal
            {
                Id = Guid.NewGuid(),
                ContribuyenteId = juanPerez.Id,
                NCF = "E310000000002",
                monto = 1000.00m,
                itbis18 = 180.00m
            }
        };

        await context.ComprobantesFiscales.AddRangeAsync(comprobantesFiscales);
        await context.SaveChangesAsync();
    }
}
