using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MVCCore.Models;
using MVCCore.Models.Enumerations;

namespace MVCCore.Data
{
    public static class SeedData
    {
        public static async Task SeedDatabase(IServiceProvider serviceProvider)
        {
            await InitializeAlbums(serviceProvider);
            await InitializeAdmin(serviceProvider);
            await InitializeStats(serviceProvider);
            await InitializeReviews(serviceProvider);
            await InitializeCovers(serviceProvider);
        }

        private static async Task InitializeCovers(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            if (context == null || context.Covers == null) throw new ArgumentNullException("Null Context");

            if (context.Covers.Any()) { return; }

            var Wedding = new CoverModel()
            {
                ID = Guid.NewGuid().ToString(),
                Category = Category.Wedding,
                DbPath = "/"
            };
            var Christening = new CoverModel()
            {
                ID = Guid.NewGuid().ToString(),
                Category = Category.Christening,
                DbPath = "/"
            };
            var CivilWedding = new CoverModel()
            {
                ID = Guid.NewGuid().ToString(),
                Category = Category.CivilWedding,
                DbPath = "/"
            };

            await context.Covers.AddRangeAsync(Wedding, Christening,CivilWedding);
            await context.SaveChangesAsync();
        }

        private static async Task InitializeAlbums(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            if (context == null || context.Albums == null)
            {
                throw new ArgumentNullException("Null Context");
            }

            if (context.Albums.Any())
            {
                return;
            }

            var topfive = Guid.NewGuid().ToString();
            var studio = Guid.NewGuid().ToString();
            var locations = Guid.NewGuid().ToString();
            var portraits = Guid.NewGuid().ToString();

            await context.Albums.AddAsync(new AlbumModel()
            {
                Id = topfive,
                Name = Category.TopFive.ToString(),
                Category = Category.TopFive,
                DbPhotos = new List<PhotoModel>()
                {
                    new PhotoModel()
                    {
                        AlbumId = topfive,
                        DbPath = @"/img/Placeholder.jpg",
                        Id = Guid.NewGuid().ToString(),
                        IsThumbnail = true
                    }
                },
            });
            await context.Albums.AddAsync(new AlbumModel()
            {
                Id = locations,
                Name = Category.Locations.ToString(),
                Category = Category.Locations,
                DbPhotos = new List<PhotoModel>()
                {
                    new PhotoModel()
                    {
                        AlbumId = locations,
                        DbPath = @"/img/category-location.jpg",
                        Id = Guid.NewGuid().ToString(),
                        IsThumbnail = true
                    }
                },
            });
            await context.Albums.AddAsync(new AlbumModel()
            {
                Id = studio,
                Name = Category.Studio.ToString(),
                Category = Category.Studio,
                DbPhotos = new List<PhotoModel>()
                {
                    new PhotoModel()
                    {
                        AlbumId = studio,
                        DbPath = @"/img/category-studio.jpg",
                        Id = Guid.NewGuid().ToString(),
                        IsThumbnail = true
                    }
                },
            });
            await context.Albums.AddAsync(new AlbumModel()
            {
                Id = portraits,
                Name = Category.Portraits.ToString(),
                Category = Category.Portraits,
                DbPhotos = new List<PhotoModel>()
                {
                    new PhotoModel()
                    {
                        AlbumId = portraits,
                        DbPath = @"/img/category-portrait.jpg",
                        Id = Guid.NewGuid().ToString(),
                        IsThumbnail = true
                    }
                },
            });
            await context.SaveChangesAsync();
        }
        private static async Task InitializeAdmin(IServiceProvider serviceProvider)
        {
            using var context = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            if (context == null || context.Users == null)
            {
                throw new ArgumentNullException("Null Context");
            }
            if(context.Users.Any())
            {
                return;
            }
            string email = "ADMIN_MAIL";
            string pw = "ADMIN_PW";

            if(await context.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser();
                user.UserName = email;
                user.Email = email;
                user.EmailConfirmed = true;

                await context.CreateAsync(user, pw);
            }
        }
        private static async Task InitializeStats(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
            if (context == null || context.Stats == null)
            {
                throw new ArgumentNullException("Null Context");
            }

            if (context.Stats.Any())
            {
                return;
            }
            await context.Stats.AddAsync(new StatsModel()
            {
                Id=Guid.NewGuid().ToString(),
                Events = 0,
                Reviews = 0,
                Trophies = 0,
            });
            await context.SaveChangesAsync();
        }
        private static async Task InitializeReviews(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
            if (context == null || context.Reviews == null)
            {
                throw new ArgumentNullException("Null Context");
            }

            if (context.Reviews.Any())
            {
                return;
            }
            await context.Reviews.AddAsync(new ReviewModel()
            {
               DbPhoto = @"wwwroot\img\review_placeholder.png",
               Id = Guid.NewGuid().ToString()
            });
            await context.SaveChangesAsync();
        }
    }
}
