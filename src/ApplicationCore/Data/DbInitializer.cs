using ApplicationCore.Data.Entities.Blog;
using ApplicationCore.Data.Identity;
using ApplicationCore.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationCore.Data
{
    public static class DbInitializer
    {
        private static IServiceProvider _serviceProvider;

        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            await _createRoles();
            await _createUsers();
            await _createTags();
            await _createArticles();
        }

        private static async Task _createRoles()
        {
            var roleManager = _serviceProvider.GetService<RoleManager<IdentityRole>>();
            var roles = new[] { "Admins" };

            foreach (var role in roles)
            {
                if (!roleManager.Roles.Any(r => r.Name == role))
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = role });
                }
            }
        }

        private static async Task _createUsers()
        {
            var userManager = _serviceProvider.GetService<UserManager<AppUser>>();
            var usernames = new[] { "Admin", "Alice", "Bob", "Joe" };
            var admins = new[] { "Admin" };
            const string password = "123456";

            foreach (var username in usernames)
            {
                if (await userManager.FindByNameAsync(username) != null)
                {
                    continue;
                }

                var newUser = new AppUser
                {
                    UserName = username,
                    Email = $"{username.ToLower()}@example.com",
                    EmailConfirmed = true,
                    CreateAt = DateTime.Now
                };

                var result = await userManager.CreateAsync(newUser, password);
                if (result.Succeeded)
                {
                    if (Array.IndexOf(admins, newUser.UserName) > -1)
                    {
                        await userManager.AddToRolesAsync(newUser, new[] { "Admins" });
                    }
                }
            }
        }

        private static async Task _createTags()
        {
            var tagRepository = _serviceProvider.GetService<ITagRepository>();

            var tags = new List<Tag>
            {
                new Tag { Name = "Linux", Image = "blog/tags/linux.png" },
                new Tag { Name = "Windows", Image = "blog/tags/windows.png" },
                new Tag { Name = "Mac OS X", Image = "blog/tags/mac_os_x.jpg" }
            };

            foreach (var tag in tags)
            {
                if (tagRepository.GetAll().Any(t => t.Name == tag.Name) is false)
                {
                    await tagRepository.CreateAsync(tag);
                }
            }
        }

        private static async Task _createArticles()
        {
            var articleRepository = _serviceProvider.GetService<IArticleRepository>();
            var tagRepository = _serviceProvider.GetService<ITagRepository>();
            var userManager = _serviceProvider.GetService<UserManager<AppUser>>();
            var owner = await userManager.FindByNameAsync("Admin");

            var articles = new List<Article>
            {
                new Article
                {
                    Title = "Primer articulo de prueba",
                    Body = "Sunt reprehenderit non elit quis ex excepteur magna officia quis esse deserunt voluptate ipsum voluptate. Non in commodo cupidatat amet proident esse occaecat. Lorem ipsum deserunt aliqua aute consequat esse. Ad sint ad aute mollit. Consectetur ullamco aliqua sunt velit mollit dolore. Ipsum enim ut amet tempor minim.\n\nSunt reprehenderit non elit quis ex excepteur magna officia quis esse deserunt voluptate ipsum voluptate. Non in commodo cupidatat amet proident esse occaecat. Lorem ipsum deserunt aliqua aute consequat esse. Ad sint ad aute mollit. Consectetur ullamco aliqua sunt velit mollit dolore. Ipsum enim ut amet tempor minim.\n\nSunt reprehenderit non elit quis ex excepteur magna officia quis esse deserunt voluptate ipsum voluptate. Non in commodo cupidatat amet proident esse occaecat. Lorem ipsum deserunt aliqua aute consequat esse. Ad sint ad aute mollit. Consectetur ullamco aliqua sunt velit mollit dolore. Ipsum enim ut amet tempor minim.",
                    OwnerId = owner.Id,
                    DefaultTagId = 1,
                    ImageHeader = "blog/articles/636593749481290183-pexels-photo-247791.png",
                    Active = true,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now
                },
                new Article
                {
                    Title = "Segundo articulo de prueba",
                    Body = "Sunt reprehenderit non elit quis ex excepteur magna officia quis esse deserunt voluptate ipsum voluptate. Non in commodo cupidatat amet proident esse occaecat. Lorem ipsum deserunt aliqua aute consequat esse. Ad sint ad aute mollit. Consectetur ullamco aliqua sunt velit mollit dolore. Ipsum enim ut amet tempor minim.\n\nSunt reprehenderit non elit quis ex excepteur magna officia quis esse deserunt voluptate ipsum voluptate. Non in commodo cupidatat amet proident esse occaecat. Lorem ipsum deserunt aliqua aute consequat esse. Ad sint ad aute mollit. Consectetur ullamco aliqua sunt velit mollit dolore. Ipsum enim ut amet tempor minim.\n\nSunt reprehenderit non elit quis ex excepteur magna officia quis esse deserunt voluptate ipsum voluptate. Non in commodo cupidatat amet proident esse occaecat. Lorem ipsum deserunt aliqua aute consequat esse. Ad sint ad aute mollit. Consectetur ullamco aliqua sunt velit mollit dolore. Ipsum enim ut amet tempor minim.",
                    OwnerId = owner.Id,
                    DefaultTagId = 1,
                    Active = true,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now
                },
                new Article
                {
                    Title = "Tercer articulo de prueba",
                    Body = "Sunt reprehenderit non elit quis ex excepteur magna officia quis esse deserunt voluptate ipsum voluptate. Non in commodo cupidatat amet proident esse occaecat. Lorem ipsum deserunt aliqua aute consequat esse. Ad sint ad aute mollit. Consectetur ullamco aliqua sunt velit mollit dolore. Ipsum enim ut amet tempor minim.\n\nSunt reprehenderit non elit quis ex excepteur magna officia quis esse deserunt voluptate ipsum voluptate. Non in commodo cupidatat amet proident esse occaecat. Lorem ipsum deserunt aliqua aute consequat esse. Ad sint ad aute mollit. Consectetur ullamco aliqua sunt velit mollit dolore. Ipsum enim ut amet tempor minim.\n\nSunt reprehenderit non elit quis ex excepteur magna officia quis esse deserunt voluptate ipsum voluptate. Non in commodo cupidatat amet proident esse occaecat. Lorem ipsum deserunt aliqua aute consequat esse. Ad sint ad aute mollit. Consectetur ullamco aliqua sunt velit mollit dolore. Ipsum enim ut amet tempor minim.",
                    OwnerId = owner.Id,
                    DefaultTagId = 2,
                    ImageHeader = "blog/articles/636593749481290183-pexels-photo-247791.png",
                    Active = true,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now
                },
                new Article
                {
                    Title = "Cuarto articulo de prueba",
                    Body = "Sunt reprehenderit non elit quis ex excepteur magna officia quis esse deserunt voluptate ipsum voluptate. Non in commodo cupidatat amet proident esse occaecat. Lorem ipsum deserunt aliqua aute consequat esse. Ad sint ad aute mollit. Consectetur ullamco aliqua sunt velit mollit dolore. Ipsum enim ut amet tempor minim.\n\nSunt reprehenderit non elit quis ex excepteur magna officia quis esse deserunt voluptate ipsum voluptate. Non in commodo cupidatat amet proident esse occaecat. Lorem ipsum deserunt aliqua aute consequat esse. Ad sint ad aute mollit. Consectetur ullamco aliqua sunt velit mollit dolore. Ipsum enim ut amet tempor minim.\n\nSunt reprehenderit non elit quis ex excepteur magna officia quis esse deserunt voluptate ipsum voluptate. Non in commodo cupidatat amet proident esse occaecat. Lorem ipsum deserunt aliqua aute consequat esse. Ad sint ad aute mollit. Consectetur ullamco aliqua sunt velit mollit dolore. Ipsum enim ut amet tempor minim.",
                    OwnerId = owner.Id,
                    DefaultTagId = 2,
                    Active = true,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now
                },
                new Article
                {
                    Title = "Quinto articulo de prueba",
                    Body = "Sunt reprehenderit non elit quis ex excepteur magna officia quis esse deserunt voluptate ipsum voluptate. Non in commodo cupidatat amet proident esse occaecat. Lorem ipsum deserunt aliqua aute consequat esse. Ad sint ad aute mollit. Consectetur ullamco aliqua sunt velit mollit dolore. Ipsum enim ut amet tempor minim.\n\nSunt reprehenderit non elit quis ex excepteur magna officia quis esse deserunt voluptate ipsum voluptate. Non in commodo cupidatat amet proident esse occaecat. Lorem ipsum deserunt aliqua aute consequat esse. Ad sint ad aute mollit. Consectetur ullamco aliqua sunt velit mollit dolore. Ipsum enim ut amet tempor minim.\n\nSunt reprehenderit non elit quis ex excepteur magna officia quis esse deserunt voluptate ipsum voluptate. Non in commodo cupidatat amet proident esse occaecat. Lorem ipsum deserunt aliqua aute consequat esse. Ad sint ad aute mollit. Consectetur ullamco aliqua sunt velit mollit dolore. Ipsum enim ut amet tempor minim.",
                    OwnerId = owner.Id,
                    DefaultTagId = 3,
                    ImageHeader = "blog/articles/636593749481290183-pexels-photo-247791.png",
                    Active = true,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now
                },
            };

            int randomTags(int min, int max)
            {
                var random = new Random();
                return random.Next(min, max + 1);
            }

            var tagsCount = tagRepository.GetAll().Count();
            foreach (var article in articles)
            {
                var tagList = new List<int>();
                while (tagList.Count < 2)
                {
                    var tagId = randomTags(1, tagsCount);
                    if (tagList.IndexOf(tagId) == -1)
                    {
                        tagList.Add(tagId);
                    }
                }

                if (articleRepository.TitleExists(article.Title) is false)
                {
                    await articleRepository.CreateAsync(tagRepository, article, tagList);
                }
            }
        }
    }
}
