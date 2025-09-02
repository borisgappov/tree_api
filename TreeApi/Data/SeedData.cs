using TreeApi.Data.Entities;

namespace TreeApi.Data
{
    public static class SeedData
    {
        /// <summary>
        /// Seeds the database with initial trees and nodes data if the database is empty
        /// </summary>
        /// <param name="context">The database context</param>
        /// <returns>A task that represents the asynchronous seeding operation</returns>
        public static async Task SeedAsync(TreeApiDbContext context)
        {
            if (context.Trees.Any())
                return;
                
            var trees = new List<Tree>
            {
                new() { Name = "Company Organization" },
                new() { Name = "File System" },
                new() { Name = "Product Categories" }
            };
            
            context.Trees.AddRange(trees);
            await context.SaveChangesAsync();
            
            var companyTree = trees[0];
            var companyNodes = new List<Node>
            {
                new() { Name = "CEO", TreeId = companyTree.Id },
                new() { Name = "CTO", TreeId = companyTree.Id },
                new() { Name = "CFO", TreeId = companyTree.Id },
                new() { Name = "HR Director", TreeId = companyTree.Id },
                new() { Name = "Marketing Director", TreeId = companyTree.Id },
                new() { Name = "Sales Director", TreeId = companyTree.Id },
                new() { Name = "Engineering Manager", TreeId = companyTree.Id },
                new() { Name = "Product Manager", TreeId = companyTree.Id },
                new() { Name = "QA Lead", TreeId = companyTree.Id },
                new() { Name = "DevOps Engineer", TreeId = companyTree.Id },
                new() { Name = "Frontend Developer", TreeId = companyTree.Id },
                new() { Name = "Backend Developer", TreeId = companyTree.Id },
                new() { Name = "UI/UX Designer", TreeId = companyTree.Id },
                new() { Name = "Data Analyst", TreeId = companyTree.Id },
                new() { Name = "Business Analyst", TreeId = companyTree.Id }
            };
            
            context.Nodes.AddRange(companyNodes);
            await context.SaveChangesAsync();
            
            var cto = companyNodes[1];
            var cfo = companyNodes[2];
            var marketing = companyNodes[4];
            var engManager = companyNodes[6];
            var productManager = companyNodes[7];
            var qaLead = companyNodes[8];
            var devOps = companyNodes[9];
            var frontend = companyNodes[10];
            var backend = companyNodes[11];
            var designer = companyNodes[12];
            var dataAnalyst = companyNodes[13];
            var businessAnalyst = companyNodes[14];
            
            engManager.ParentId = cto.Id;
            productManager.ParentId = cto.Id;
            qaLead.ParentId = engManager.Id;
            devOps.ParentId = engManager.Id;
            frontend.ParentId = engManager.Id;
            backend.ParentId = engManager.Id;
            designer.ParentId = productManager.Id;
            dataAnalyst.ParentId = cfo.Id;
            businessAnalyst.ParentId = marketing.Id;
            
            await context.SaveChangesAsync();
            
            var fileTree = trees[1];
            var fileNodes = new List<Node>
            {
                new() { Name = "Root", TreeId = fileTree.Id },
                new() { Name = "Documents", TreeId = fileTree.Id },
                new() { Name = "Pictures", TreeId = fileTree.Id },
                new() { Name = "Music", TreeId = fileTree.Id },
                new() { Name = "Videos", TreeId = fileTree.Id },
                new() { Name = "Downloads", TreeId = fileTree.Id },
                new() { Name = "Work", TreeId = fileTree.Id },
                new() { Name = "Personal", TreeId = fileTree.Id },
                new() { Name = "Projects", TreeId = fileTree.Id },
                new() { Name = "Reports", TreeId = fileTree.Id },
                new() { Name = "Presentations", TreeId = fileTree.Id },
                new() { Name = "Photos", TreeId = fileTree.Id },
                new() { Name = "Screenshots", TreeId = fileTree.Id },
                new() { Name = "Albums", TreeId = fileTree.Id },
                new() { Name = "Movies", TreeId = fileTree.Id }
            };
            
            context.Nodes.AddRange(fileNodes);
            await context.SaveChangesAsync();
            
            var root = fileNodes[0];
            var documents = fileNodes[1];
            var pictures = fileNodes[2];
            var music = fileNodes[3];
            var videos = fileNodes[4];
            var downloads = fileNodes[5];
            var work = fileNodes[6];
            var personal = fileNodes[7];
            var projects = fileNodes[8];
            var reports = fileNodes[9];
            var presentations = fileNodes[10];
            var photos = fileNodes[11];
            var screenshots = fileNodes[12];
            var albums = fileNodes[13];
            var movies = fileNodes[14];
            
            documents.ParentId = root.Id;
            pictures.ParentId = root.Id;
            music.ParentId = root.Id;
            videos.ParentId = root.Id;
            downloads.ParentId = root.Id;
            work.ParentId = documents.Id;
            personal.ParentId = documents.Id;
            projects.ParentId = work.Id;
            reports.ParentId = work.Id;
            presentations.ParentId = work.Id;
            photos.ParentId = pictures.Id;
            screenshots.ParentId = pictures.Id;
            albums.ParentId = music.Id;
            movies.ParentId = videos.Id;
            
            await context.SaveChangesAsync();
            
            var productTree = trees[2];
            var productNodes = new List<Node>
            {
                new() { Name = "Electronics", TreeId = productTree.Id },
                new() { Name = "Clothing", TreeId = productTree.Id },
                new() { Name = "Books", TreeId = productTree.Id },
                new() { Name = "Home & Garden", TreeId = productTree.Id },
                new() { Name = "Sports", TreeId = productTree.Id },
                new() { Name = "Computers", TreeId = productTree.Id },
                new() { Name = "Smartphones", TreeId = productTree.Id },
                new() { Name = "Tablets", TreeId = productTree.Id },
                new() { Name = "Laptops", TreeId = productTree.Id },
                new() { Name = "Desktops", TreeId = productTree.Id },
                new() { Name = "Men's Clothing", TreeId = productTree.Id },
                new() { Name = "Women's Clothing", TreeId = productTree.Id },
                new() { Name = "Kids' Clothing", TreeId = productTree.Id },
                new() { Name = "Fiction", TreeId = productTree.Id },
                new() { Name = "Non-Fiction", TreeId = productTree.Id }
            };
            
            context.Nodes.AddRange(productNodes);
            await context.SaveChangesAsync();
            
            var electronics = productNodes[0];
            var clothing = productNodes[1];
            var books = productNodes[2];
            var computers = productNodes[5];
            var smartphones = productNodes[6];
            var tablets = productNodes[7];
            var laptops = productNodes[8];
            var desktops = productNodes[9];
            var mensClothing = productNodes[10];
            var womensClothing = productNodes[11];
            var kidsClothing = productNodes[12];
            var fiction = productNodes[13];
            var nonFiction = productNodes[14];
            
            computers.ParentId = electronics.Id;
            smartphones.ParentId = electronics.Id;
            tablets.ParentId = electronics.Id;
            laptops.ParentId = computers.Id;
            desktops.ParentId = computers.Id;
            mensClothing.ParentId = clothing.Id;
            womensClothing.ParentId = clothing.Id;
            kidsClothing.ParentId = clothing.Id;
            fiction.ParentId = books.Id;
            nonFiction.ParentId = books.Id;
            
            await context.SaveChangesAsync();
            
            await SeedPartnersAsync(context);
        }
        
        /// <summary>
        /// Seeds the database with initial partners data if the database is empty
        /// </summary>
        /// <param name="context">The database context</param>
        /// <returns>A task that represents the asynchronous seeding operation</returns>
        public static async Task SeedPartnersAsync(TreeApiDbContext context)
        {
            if (context.Partners.Any())
                return;
                
            var partners = new List<Partner>
            {
                new() { Code = "PARTNER001" },
                new() { Code = "PARTNER002" },
                new() { Code = "PARTNER003" },
                new() { Code = "PARTNER004" },
                new() { Code = "PARTNER005" }
            };
            
            context.Partners.AddRange(partners);
            await context.SaveChangesAsync();
        }
    }
}
