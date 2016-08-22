using Krosbook.Models.Users;
using Krosbook.Models.Reservation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;

namespace Krosbook.Models
{
    /// <summary>
    /// Class for fill init data.
    /// </summary>
    public static class DbInitializer
    {
        private static ApplicationDbContext _context;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            _context = serviceProvider.GetService<ApplicationDbContext>();

            InitializeUserRoles();
         //   InitRoomUser();
        }
        /*
        private static void InitRoomUser() {
            _context.Room.Add(new Rooms.Room()
            { Name="MyRoom",Description="moja miestnost",Type="premietacia"});
           // _context.SaveChanges();

            _context.Add(new RoomReservation() {
                RoomId = 1,
                UserId=1,
                dateTime=DateTime.Now
            });
            _context.SaveChanges();

        }
        */

        private static void InitializeUserRoles()
        {
            if (!_context.Role.Any())
            {
                // Create roles
                _context.Role.AddRange(new Role[]
                {
                new Role()
                {
                    Name="Admin"
                },
                //new Role()
                //{
                //    Name="User"
                //}
                });

                _context.SaveChanges();
            }

          if (!_context.User.Any())
           {
                // Create users
             _context.User.Add(new User()
                {
                    Email = "demo@demo.com",
                    Name = "Adam",
                    Surname = "Novák",
                    PasswordHash = "$2a$06$U5imXrHtrfmHjDKUkxsWKepaY2xVlhlCE.PlbkS9PBkp4npUCQ9zi", //string.Empty teraz tam je demo
                    IsLocked = false,
                    DateCreated = DateTime.Now,
                    Photo = GetDefaultAvatar()
                });
                _context.SaveChanges();
                _context.User.Add(new User()
                {
                    Email = "user@user.sk",
                    Name = "Boris",
                    Surname = "Kollár",
                    PasswordHash = "$2a$06$gF/DQUqo0z8xD3kzEketk.XGRt8PZP3fvbEUVbOcmWxeVI8jaH0OG", //string.Empty teraz tam je user
                    IsLocked = false,
                    DateCreated = DateTime.Now,
                    Photo = GetDefaultAvatar()
                });
                _context.SaveChanges();
                // Add role for user
                _context.UserRole.AddRange(new UserRole[] {
                    new UserRole() {
                        RoleId = 1, // admin
                        UserId = 1  // demo
                    }
                });
                _context.SaveChanges();
                _context.UserRole.AddRange(new UserRole[] {
                    new UserRole() {
                        RoleId = 1, // admin
                        UserId = 2  // demo
                    }
                });

           
              _context.SaveChanges();
          }
        }

        public static byte[] GetDefaultAvatar()
        {
            string base64Image = 
                @"iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAABHNCSVQICAgIfAhkiAAAAAlwSFlz
                AAALEwAACxMBAJqcGAAABxZJREFUeJztm2tsFUUUx39F+qYilIemtCo+SiOiID6CqMH4NoBUo9Ev
                fiAxhg8owcQYNFERpD6iIYji6wMxhoBAfCRGUUQjBqGiICBqUmspFEvLowXio6V+ODvZs7e79+7M
                3Xv9YP/JZufenfOY2ZkzZ86chQEMYAD/ZxTkQcZgYDIwFagDaoFqoAIY4tU5DnQD+4C9wE/A18B3
                QE8edEwcxUA9sA7oAvocry5gLTDL45k4kh4BI4F5wIPAsIg6nUATcAx56yCjYShwHjA8gu4w8Crw
                MtCRkL6JYQjQAJyk/1vcCiwGbgQqY/AaAdwEPAs0hvA74fErT7QFWaAeaCWoZDuwELggAf61wCLg
                UIqMFmBmAvydUQq8nqLUQWAuUJYDeeXI9GpPkbkcKMmBvLSoBnYoJXqBpcg8zjWGIY0+peRvB6ry
                IBuQpaxFCW8FrsmXcIVpQJvSoxmZLjlFHcG5uAkxWv8VRiP+grY9Vp1gswxWA5u9O8B64D7gT0se
                xiGqwrfkJ5CRtBtp0AELnmXAKmC69/t3YIolj4woJTjn1yEeXhwMAu4FttB/SYu6vgLuIP4LGgx8
                SNAmJGoYtbXfZMG8Bhk1rp7gp8ComLLKUmS9EpMuI+oV01bE24uD8wkaqV7gfWA2cClizU9D3l4l
                MAnxIDcQ7IQmYExMmWciS7GhnZ6+emZU4Ds5vcS39mXAHqXIZuyM02TgR0XfCBTFpL0ef4lsJkuP
                sUEpsdSC7ilF9wluG5kK4FvFZ74F7QpF94yDbEDmnvHtDxLfySkHjnp0HcTz/6NQg2yV+4A/iN+R
                w/GX6+OuOizC78W5FnR3K7oFLoJT8LziN8OC7hFF97St0GJk+2mcCxvfXq8YY20Fh2CC4rfMgm4I
                svU2IzGuDQGCln+hDSGwzaNLyhEpAI54PL+xpNU2zGrnuE4R2m5pzdz70pIuHYwxbLOkG4/fjjVx
                iQqRSE0f8jZt0YPvLSaFjzyefzvQ7vRojyJ+RwCDQgguww9WbnAQaIT0OtBGwQRG47rfGqYNQ4GJ
                qQ/DOmCqKn/hILBLCUwKJk541IF2oyr3c+TCOqBOlbc7CDTGr8aBNgqGl4th/V6V61IfhnWAcVk7
                vcsWzd79XELmnAOK8LfgTQ70bfjR537ueFgHZCMM4FfvXkQyo2Asvp6/ONCbDRVh+oR1QIV3P+Yg
                DORUx2CcIw8NPWx/duRh2lKR+iCsA8wK0B3yLA72qPJ4Rx4aF6nybkcexjDH6gAD12Vslypf7MhD
                Q3firsha6RHZlrAOOO7dXWP7nfjW+hJHHhoTvHsz/pu0hWlLv1Ed1gGmUjbR3p3efRzZxeZK8V3x
                nekqZoBpS6wO2Ofds7Hgjd69CNkdunhwhcDbinZrFvqc7d1b4lR+E38DEXXCmwmjCB5hfYa/vMZB
                NRJ8NfT7stBltOKzIg7BfEVwg6NQgCvwt7FmMzKb9Ia3ALg/ha4d3w644DbFa14cgisVgXUkJQUX
                Iq6ojvJuQTZcqbic/mcH2xCPMhvomMDkOASD8bM6GjPUjYNC4HHkBMko0gVcpepcjR/76wP+QQIx
                VlGcCJjo8hEsXPO1SpkkwloA5wDvKL6r1LP31P/rCdm0OKJO8V1tQzhLETqHlUNwuuL7rvp/tfq/
                NEF5zym+NgFVivEDiu0JKqU74C31/0r1f1LJFbGColEW+S8kIQnkKOyBBBQaBNylfreq8n5Vrk+j
                lw3m4AdSluEQThuBHFv3IYHOM2LSFSLJC08iZ4G7kRB7L/5bPkVwnzBJPTPHcB3IifRa4AngWuI7
                VJX4Yf1uojPPMmKxUipTTH4M8BL+sEt3PRZCvyAGXQfwIpnTYd5QNFkt5eUE02GmhdQpQByMsBS5
                HiRAshGx9EuRJS8KU5A8wDXA50gAxESZ9XXSkxmWP3CzqvcbCdiUmYrhfsS11FieotwBYAkSgEzC
                eJZ4vJZ48rWs1ByAKuQM0Ty/PQH5gBhEw3QzfsPuUf+fAB4iGeclCkXAwwRH253es3L8U6k+7E6z
                M6KEoEv7AeLYmPnegwzffOE6/ByAQ8jO9WOl3zZykFtchQQljJBuVbY9P0wC2snRujQBZ+VKaC39
                MzUbkaUv3ygmmLjVh8z/JNJz06KW4EhoI7ttsytuIWjwmshD4w2qkFMj3fuvkYXDYYFKgkEbMwpz
                NuyjUIIsQ1qRTiSgkotU9grgUYLBEmPtc/IxRVzMQLIzUzuigWRyd+uAF/BdW+3kJLbOZ4tyZNus
                gxrm2oF8/HAr8XIMRyENa0DOAVL5dSPubSK7xqQ/malEnKE5RGdmdSLu9SHEoSlAGjMSid6m+9Rm
                GTLkDyencm5QhLjQa/BT5lyuI0iwZDo5Wmrz9dncRIKfzdUgRs2c1XV7Vwvy2dxeJGH6B5LNNBnA
                AAYwgAD+BSgBe09B+XKTAAAAAElFTkSuQmCC";

            byte[] bytes = Convert.FromBase64String(base64Image);

            //Image image;
            //using (MemoryStream ms = new MemoryStream(bytes))
            //{
            //    image = Image.FromStream(ms);
            //}

            return bytes;
        }
    }
}
