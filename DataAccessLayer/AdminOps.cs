using DataBase.Model;
using DataBase.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace DataAccessLayer
{
    public class AdminOps
    {
        #region font
                private static string Prompt = @"
███╗   ███╗ ██████╗ ██╗   ██╗██╗███████╗███╗   ██╗███████╗██╗  ██╗
████╗ ████║██╔═══██╗██║   ██║██║██╔════╝████╗  ██║██╔════╝╚██╗██╔╝
██╔████╔██║██║   ██║██║   ██║██║█████╗  ██╔██╗ ██║█████╗   ╚███╔╝   
██║╚██╔╝██║██║   ██║╚██╗ ██╔╝██║██╔══╝  ██║╚██╗██║██╔══╝   ██╔██╗ 
██║ ╚═╝ ██║╚██████╔╝ ╚████╔╝ ██║███████╗██║ ╚████║███████╗██╔╝ ██╗
╚═╝     ╚═╝ ╚═════╝   ╚═══╝  ╚═╝╚══════╝╚═╝  ╚═══╝╚══════╝╚═╝  ╚═╝";

        #endregion

        #region option
        public readonly string[] UpdateMovieOption = { "Movie Title", "Movie Duration", "Movie Release Date", "Movie Rating", "Go Back" };
        public readonly string[] UserManagementOption = { "Display list of all Users", "Delete User", "Update User's Data", "Go Back"};
        public readonly string[] UpdateUserOption = { "Update User's Name", "Update User's Phone Number", "Go Back"};
        #endregion

        #region Add Movie
        public void AddMovie()
        {
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine(Prompt + "\n\n Admin Rights -  Add Movie");
            ResetColor();
            Write("\n Enter Title : ");
            string Title = ReadLine().ToLower().Trim();
            while (!UtilityClass.ValidMovieTitle(Title))
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine("\n Invalid Title");
                ResetColor();
                Write("\n Enter Title again : ");
                Title = ReadLine().ToLower().Trim();
            }


            Write("\n Enter duration in Minutes : ");
            string DurationTemp = ReadLine().Trim();
            while (!UtilityClass.ValidMovieDuration(DurationTemp))
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine("\n Please enter valid duration");
                ResetColor();
                Write("\n Enter duration in Minutes : ");
                DurationTemp = ReadLine().Trim();
            }
            int Duration = Convert.ToInt32(DurationTemp);

            Write("\n Enter release date(dd/mm/yyyy) : ");
            string ReleaseDate = ReadLine().Trim();
            while (!UtilityClass.ValidDate(ReleaseDate))
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine("\n Invalid date");
                ResetColor();
                Write("\n Enter valid release date(dd/mm/yyyy) : ");
                ReleaseDate = ReadLine().Trim();
            }


            Write("\n Enter the Plot of movie : ");
            string Plot = ReadLine().ToLower().Trim();


            Write("\n Enter rating(Between 1 to 10) : ");
            string RatingTemp = ReadLine().Trim();
            while (!UtilityClass.ValidRating(RatingTemp))
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine("\n Invalid input");
                ResetColor();
                Write("\n Enter valid rating(Between 1 to 10) : ");
                RatingTemp = ReadLine().Trim();
            }
            float Rating = (float)Convert.ToDecimal(RatingTemp);

            bool isMoreGenre = true;
            List<string> GenreList = new List<string>();
            List<string> ActorList = new List<string>();
            bool isMoreActor = true;
            string GenreChoice;
            while (isMoreGenre)
            {
                Write("\n Enter Genre : ");
                string Genre = ReadLine().ToLower().Trim();
                while (!UtilityClass.ValidName(Genre))
                {
                    ForegroundColor = ConsoleColor.Red;
                    WriteLine("\n Please enter valid genre");
                    ResetColor();
                    Write("\n Enter Genre : ");
                    Genre = ReadLine().ToLower().Trim();
                }
                GenreList.Add(Genre);
                Write("\n Do you want to add another genre?(y/n) : ");
                GenreChoice = ReadLine().ToLower().Trim();
                while (!UtilityClass.ValidChoice(GenreChoice))
                {
                    ForegroundColor = ConsoleColor.Red;
                    WriteLine("\n Invalid input");
                    ResetColor();
                    Write("\n Enter valid choice(y/n) : ");
                    GenreChoice = ReadLine().Trim();
                }
                if (GenreChoice.ToString().ToLower() == "n")
                {
                    isMoreGenre = false;
                }
            }
            string ActorChoice;
            while (isMoreActor)
            {
                Write("\n Enter Actor Name : ");
                string Name = ReadLine().ToLower().Trim();
                while (!UtilityClass.ValidName(Name))
                {
                    ForegroundColor = ConsoleColor.Red;
                    Write("\n Please enter valid actor name : ");
                    ResetColor();
                    Write("\n Enter Actor Name : ");
                    Name = ReadLine().ToLower().Trim();
                }
                ActorList.Add(Name);
                Write("\n Do you want to add another actor?(y/n) : ");
                ActorChoice = ReadLine().ToLower().Trim();
                while (!UtilityClass.ValidChoice(ActorChoice))
                {
                    ForegroundColor = ConsoleColor.Red;
                    WriteLine("\n Invalid input");
                    ResetColor();
                    Write("\n Enter valid choice(y/n) : ");
                    ActorChoice = ReadLine().Trim();
                }
                if (ActorChoice.ToString().ToLower() == "n" )
                {
                    isMoreActor = false;
                }
            }
            MoviesInfo Movie = new MoviesInfo
            {
                Title = Title,
                Duration = Duration,
                Ratting = Rating,
                ReleaseDate = Convert.ToDateTime(ReleaseDate),
                Plot = Plot
            };
            using (UserMovieDBContext db = new UserMovieDBContext())
            {
                db.Movies.Add(Movie);
                db.SaveChanges();
            }
            using (UserMovieDBContext db = new UserMovieDBContext())
            {
                foreach (string GenreItem in GenreList)
                {
                    Genre Genre = db.Genres.FirstOrDefault(x => x.GenreName.ToLower().Trim() == GenreItem.ToLower().Trim());
                    if (Genre != null)
                    {
                        db.Movies.FirstOrDefault(x => x.Title.ToLower().Trim() == Title.ToLower().Trim()).Genre.Add(Genre);
                    }
                    else
                    {
                        db.Movies.FirstOrDefault(x => x.Title.ToLower().Trim() == Title.ToLower().Trim()).Genre.Add(new Genre { GenreName = GenreItem.ToLower().Trim() });
                    }
                    db.SaveChanges();
                }
            }
            using (UserMovieDBContext db = new UserMovieDBContext())
            {
                foreach (string ActorItem in ActorList)
                {
                    StarCast Actor = db.StarCasts.FirstOrDefault(x => x.ActorName.ToLower().Trim() == ActorItem.ToLower().Trim());
                    if (Actor != null)
                    {
                        db.Movies.FirstOrDefault(x => x.Title.ToLower().Trim() == Title.ToLower().Trim()).StarCasts.Add(Actor);
                    }
                    else
                    {
                       db.Movies.FirstOrDefault(x => x.Title.ToLower().Trim() == Title.ToLower().Trim()).StarCasts.Add(new StarCast { ActorName = ActorItem.ToLower().Trim() });
                    }
                    db.SaveChanges();
                }
            }
            ForegroundColor = ConsoleColor.Green;
            WriteLine("\n Movie Added.");
            ResetColor();
        }
        #endregion

        #region Delete Movie 
        public void DeleteMovie()
        {
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine(Prompt + "\n\n Admin Rights -  Delete Movie");
            ResetColor();
            MoviesInfo MoviesInfoObj;
            Write("\n Enter the movie title, you want to remove : ");
            string Title = ReadLine().ToLower().Trim();
            Write("\n Are you sure you want to delete..(y/n) : ");
            string Choice = ReadLine().ToLower().Trim();
            while (!UtilityClass.ValidChoice(Choice))
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine("\n Invalid input");
                ResetColor();
                Write("\n Enter valid choice(y/n) : ");
                Choice = ReadLine().Trim();
            }
            if (Choice == "y" || Choice == "Y")
            {
                using (UserMovieDBContext dBContext = new UserMovieDBContext())
                {
                    MoviesInfoObj = (from Movie in dBContext.Movies where Movie.Title == Title select Movie).FirstOrDefault();
                    if (MoviesInfoObj == null)
                    {
                        ForegroundColor = ConsoleColor.Red;
                        WriteLine("\n Enter valid movie Title, Try Again");
                        ResetColor();
                        ForegroundColor = ConsoleColor.Cyan;
                        WriteLine("\n\n Press any key to re-enter movie name");
                        ReadLine();
                        ResetColor();
                        Clear();
                        DeleteMovie();
                    }
                    else
                    {
                        dBContext.Movies.Remove(MoviesInfoObj);
                        dBContext.SaveChanges();
                        ForegroundColor = ConsoleColor.Green;
                        WriteLine("\n Movie Deleted");
                        ResetColor();
                        ForegroundColor = ConsoleColor.Cyan;
                        WriteLine("\n\n Press any key to return");
                        ReadLine();
                        ResetColor();
                    }
                }
            }
            else
            {
                Clear();
                DeleteMovie();
            }

        }
        #endregion

        #region Update Movie
        public void UpdateMovie()
        {
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine(Prompt + "\n\n Admin Rights -  Update Movie");
            ResetColor();
            Write("\n Enter Title Of Movie : ");
            string MovieTitle = ReadLine().ToLower().Trim();
            while (string.IsNullOrWhiteSpace(MovieTitle))
            {
                Write("\n Enter Valid Movie name : ");
                MovieTitle = ReadLine().ToLower().Trim();
            }
            using (UserMovieDBContext db = new UserMovieDBContext())
            {
                MoviesInfo Movie = db.Movies.Where(m => m.Title.Equals(MovieTitle)).FirstOrDefault();
                if (Movie == null)
                {
                    WriteLine("No such Movie found...");
                }
                else
                {
                    int Selectedindex;
                    do
                    {
                        PersonOps PersonOpsObj = new PersonOps();
                        Menu MainMenu = new Menu(UpdateMovieOption);
                        Selectedindex = MainMenu.Run();

                        switch (Selectedindex)
                        {
                            case 0:
                                Clear();
                                ForegroundColor = ConsoleColor.Yellow;
                                WriteLine(Prompt + "\n\n Admin Rights -  Update Movie");
                                ResetColor();
                                Write("\n Enter Title Of Movie : ");
                                string NewTitle = ReadLine().ToLower().Trim();
                                while (string.IsNullOrWhiteSpace(NewTitle))
                                {
                                    Write("\n Enter Valid Movie name : ");
                                    NewTitle = ReadLine().ToLower().Trim();
                                }
                                Movie.Title = NewTitle;
                                db.SaveChanges();
                                ForegroundColor = ConsoleColor.Green;
                                WriteLine("\n Movie Title Updated");
                                ResetColor();
                                ForegroundColor = ConsoleColor.Cyan;
                                WriteLine("\n\n Press any key to return back");
                                ResetColor();
                                ReadLine();
                                break;
                            case 1:
                                Clear();
                                ForegroundColor = ConsoleColor.Yellow;
                                WriteLine(Prompt + "\n\n Admin Rights -  Update Movie");
                                ResetColor();
                                Write("\n Enter duration in Minutes : ");
                                string DurationTemp = ReadLine().Trim();
                                while (!UtilityClass.ValidMovieDuration(DurationTemp))
                                {
                                    ForegroundColor = ConsoleColor.Red;
                                    WriteLine("\n Please enter valid duration");
                                    ResetColor();
                                    Write("\n Enter duration in Minutes : ");
                                    DurationTemp = ReadLine().Trim();
                                }
                                int newDuration = Convert.ToInt32(DurationTemp);
                                Movie.Duration = newDuration;
                                db.SaveChanges();
                                ForegroundColor = ConsoleColor.Green;
                                WriteLine("\n Movie Duration Updated");
                                ResetColor();
                                ForegroundColor = ConsoleColor.Cyan;
                                WriteLine("\n\n Press any key to return back");
                                ResetColor();
                                ReadLine();
                                break;
                            case 2:
                                Clear();
                                ForegroundColor = ConsoleColor.Yellow;
                                WriteLine(Prompt + "\n\n Admin Rights -  Update Movie");
                                ResetColor();
                                Write("\n Enter release date(dd/mm/yyyy) : ");
                                string ReleaseDate = ReadLine().Trim();
                                while (!UtilityClass.ValidDate(ReleaseDate))
                                {
                                    ForegroundColor = ConsoleColor.Red;
                                    WriteLine("\n Invalid date");
                                    ResetColor();
                                    Write("\n Enter valid release date(dd/mm/yyyy) : ");
                                    ReleaseDate = ReadLine().Trim();
                                }
                                DateTime NewReleaseDate = Convert.ToDateTime(ReleaseDate);
                                Movie.ReleaseDate = NewReleaseDate;
                                db.SaveChanges();
                                ForegroundColor = ConsoleColor.Green;
                                WriteLine("\n Movie Release Date Updated");
                                ResetColor();
                                ForegroundColor = ConsoleColor.Cyan;
                                WriteLine("\n\n Press any key to return back");
                                ResetColor();
                                ReadLine();
                                break;
                            case 3:
                                Clear();
                                ForegroundColor = ConsoleColor.Yellow;
                                WriteLine(Prompt + "\n\n Admin Rights -  Update Movie");
                                ResetColor();
                                Write("\n Enter rating(Between 1 to 10) : ");
                                string RatingTemp = ReadLine().Trim();
                                while (!UtilityClass.ValidRating(RatingTemp))
                                {
                                    ForegroundColor = ConsoleColor.Red;
                                    WriteLine("\n Invalid input");
                                    ResetColor();
                                    Write("\n Enter valid rating(Between 1 to 10) : ");
                                    RatingTemp = ReadLine().Trim();
                                }
                                float NewRating = (float)Convert.ToDecimal(RatingTemp);
                                Movie.Ratting = NewRating;
                                db.SaveChanges();
                                ForegroundColor = ConsoleColor.Green;
                                WriteLine("\n Movie Rating Updated");
                                ResetColor();
                                ForegroundColor = ConsoleColor.Cyan;
                                WriteLine("\n\n Press any key to return back");
                                ResetColor();
                                ReadLine();
                                break;
                            case 4:
                                PersonOpsObj.AdminMenu();
                                break;
                        }

                    } while (Selectedindex < 5);
                   
                }
            }
        }
        #endregion

        #region User Management
        public void UserManagement()
        {
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine(Prompt + "\n\n Admin Rights - User Management \n\n What Would you like to perform ?\n");
            ResetColor();
            Clear();
            int Selectedindex;
            do
            {
                Menu MainMenu = new Menu(UserManagementOption);
                PersonOps PersonOpsObj = new PersonOps();
                Selectedindex = MainMenu.Run();

                switch (Selectedindex)
                {
                    case 0:
                        Clear();
                        ForegroundColor = ConsoleColor.Yellow;
                        WriteLine(Prompt + "\n\n Admin Rights -  User Management");
                        ResetColor();
                        ListAllUsers();
                        ForegroundColor = ConsoleColor.Cyan;
                        WriteLine("\n\n Press any key to return back");
                        ReadLine();
                        ResetColor();
                        break;
                    case 1:
                        Clear();
                        ForegroundColor = ConsoleColor.Yellow;
                        WriteLine(Prompt + "\n\n Admin Rights -  User Management");
                        ResetColor();
                        DeleteUser();
                        ForegroundColor = ConsoleColor.Cyan;
                        WriteLine("\n Press any key to return back");
                        ReadLine();
                        ResetColor();
                        break;
                    case 2:
                        Clear();
                        ForegroundColor = ConsoleColor.Yellow;
                        WriteLine(Prompt + "\n\n Admin Rights -  User Management");
                        ResetColor();
                        UpdateUser();
                        ForegroundColor = ConsoleColor.Cyan;
                        WriteLine("\n Press any key to return back");
                        ReadLine();
                        ResetColor();
                        break;
                    case 3:
                        PersonOpsObj.AdminMenu();
                        break;
                }
            } while (Selectedindex < 4);

        }
        #endregion

        #region Update User
        private void UpdateUser()
        {
            
            string Email;
            while (true)
            {
                Write("\n Enter User's Email Id : ");
                Email = ReadLine().ToLower().Trim();

                if (!UtilityClass.ValidEmail(Email))
                {
                    ForegroundColor = ConsoleColor.Red;
                    WriteLine("\n Enter Valid Email");
                    ResetColor();
                }
                else
                {
                    if (!UtilityClass.isEmailExist(Email))
                    {
                        ForegroundColor = ConsoleColor.Red;
                        WriteLine("\n Email id doesn't Exists..");
                        ResetColor();
                    }
                    else
                    {
                        break;
                    }
                }
            }
            using (UserMovieDBContext db = new UserMovieDBContext())
            {
                UserInfo User = db.Users.Where(u => u.Email.Equals(Email)).First();
                int Selectedindex;
                do
                {
                   
                    PersonOps PersonOpsObj = new PersonOps();
                    Menu MainMenu = new Menu(UpdateUserOption);

                    ResetColor();
                    Selectedindex = MainMenu.Run();

                    switch (Selectedindex)
                    {
                        case 0:
                            Clear();
                            ForegroundColor = ConsoleColor.Yellow;
                            WriteLine(Prompt + "\n\nAdmin Rights - Update User\n ");
                            ResetColor();
                            Write("\n Enter new name :");
                            string Name = ReadLine().ToLower().Trim();
                            while (!UtilityClass.ValidName(Name))
                            {
                                ForegroundColor = ConsoleColor.Red;
                                WriteLine("Please enter valid name");
                                ResetColor();
                                WriteLine("Enter name");
                                Name = ReadLine().ToLower().Trim();
                            }
                            User.Name = Name;
                            db.SaveChanges();
                            ForegroundColor = ConsoleColor.Green;
                            WriteLine("\n User name is Updated");
                            ResetColor();
                            ForegroundColor = ConsoleColor.Cyan;
                            WriteLine("\n\n Press any key to return back");
                            ResetColor();
                            ReadLine();
                            break;
                        case 1:
                            Clear();
                            ForegroundColor = ConsoleColor.Yellow;

                            WriteLine(Prompt + "\n\nAdmin Rights - Update User\n ");
                            ResetColor();
                            Write("\n Enter new phone number :");
                            string PhoneNo = ReadLine().ToLower().Trim();
                            while (!UtilityClass.ValidPhone(PhoneNo))
                            {
                                ForegroundColor = ConsoleColor.Red;
                                WriteLine("Invalid Phone number");
                                ResetColor();
                                WriteLine("Enter your Valid phone number");
                                PhoneNo = ReadLine().ToLower().Trim();
                            }
                            User.PhoneNumber = PhoneNo;
                            db.SaveChanges();
                            ForegroundColor = ConsoleColor.Green;
                            WriteLine("\n User phone number is Updated");
                            ResetColor();
                            ForegroundColor = ConsoleColor.Cyan;
                            WriteLine("\n\n Press any key to return back");
                            ResetColor();
                            ReadLine();
                            break;
                        case 2:
                            Clear();
                            PersonOpsObj.AdminMenu();
                            break;
                        default:
                            break;
                    }

                } while (Selectedindex < 3);
            }
        }
        #endregion

        #region Delete User
        private void DeleteUser()
        {
            string Email;

            while (true)
            {
                Write("\n Enter your Email Id : ");
                Email = ReadLine().ToLower().Trim();

                if (!UtilityClass.ValidEmail(Email))
                {
                    ForegroundColor = ConsoleColor.Red;
                    WriteLine("\n Enter Valid Email");
                    ResetColor();
                }
                else
                {
                    if (!UtilityClass.isEmailExist(Email))
                    {
                        ForegroundColor = ConsoleColor.Red;
                        WriteLine("\n Email id doesn't Exists..");
                        ResetColor();
                    }
                    else
                    {
                        break;
                    }
                }
            }

            using (UserMovieDBContext db = new UserMovieDBContext())
            {
                UserInfo User = db.Users.Where(u => u.Email.Equals(Email)).First();
                if (User != null)
                {
                    db.Users.Remove(User);
                    db.SaveChanges();
                    ForegroundColor = ConsoleColor.Green;
                    WriteLine("\n User is Deleted");
                    ResetColor();
                }
            }
        }
        #endregion

        #region List All User
        private void ListAllUsers()
        {
            using (UserMovieDBContext db = new UserMovieDBContext())
            {
                ForegroundColor = ConsoleColor.Blue;
                WriteLine("\n User Name \t\t  User Email \t\t\tPhone Number \tDate of Birth");
                ResetColor();
                foreach (UserInfo user in db.Users)
                {
                    Write("\n\n --------------------------------------------------------------------------------------\n\n " + user.Name.PadRight(25) + user.Email.PadRight(30) +  user.PhoneNumber + " \t" + user.DateOfBirth);
                }
            }
        }
        #endregion
    }
}