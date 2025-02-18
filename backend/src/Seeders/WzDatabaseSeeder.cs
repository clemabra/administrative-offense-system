using src.Models.Wz;

namespace src.Seeders
{
    public static class WzDatabaseSeeder
    {
        public static void Seed(WzDbContext context)
        {
            context.Database.EnsureDeleted(); // deletes existing database
            context.Database.EnsureCreated(); // recreates database

            var entities = new JsonWzEntity[]
            {
                new () { Fallnummer = "0", Weiserzeichen = "1", },
                new () { Fallnummer = "1", Weiserzeichen = "2", },
                new () { Fallnummer = "2", Weiserzeichen = "3", },
                new () { Fallnummer = "3", Weiserzeichen = "4", },
                new () { Fallnummer = "4", Weiserzeichen = "5", },
                new () { Fallnummer = "5", Weiserzeichen = "6", },
                new () { Fallnummer = "6", Weiserzeichen = "7", },
                new () { Fallnummer = "7", Weiserzeichen = "8", },
                new () { Fallnummer = "8", Weiserzeichen = "9", },
                new () { Fallnummer = "9",Weiserzeichen = "10", },
            };

            var dtoEntities = entities.Select(entity =>
            {
                var dto = new DtoWzEntity();
                dto.UpdateEntity(entity);
                return dto;
            }).ToArray();

            // add records to database
            context.WzEntities.AddRange(dtoEntities);
            context.SaveChanges();
        }
    }
}