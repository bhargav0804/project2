using DataBase.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace DataAccessLayer
{
    public class UserOps
    {
        #region font

        private static readonly string Prompt = @"
███╗   ███╗ ██████╗ ██╗   ██╗██╗███████╗███╗   ██╗███████╗██╗  ██╗
████╗ ████║██╔═══██╗██║   ██║██║██╔════╝████╗  ██║██╔════╝╚██╗██╔╝
██╔████╔██║██║   ██║██║   ██║██║█████╗  ██╔██╗ ██║█████╗   ╚███╔╝   
██║╚██╔╝██║██║   ██║╚██╗ ██╔╝██║██╔══╝  ██║╚██╗██║██╔══╝   ██╔██╗   
██║ ╚═╝ ██║╚██████╔╝ ╚████╔╝ ██║███████╗██║ ╚████║███████╗██╔╝ ██╗
╚═╝     ╚═╝ ╚═════╝   ╚═══╝  ╚═╝╚══════╝╚═╝  ╚═══╝╚══════╝╚═╝  ╚═╝";


        #endregion

        #region option string
        private readonly string[] OrderingOptions = { "Ascending by Movie Name", "Descending by Release Date" ,"Go Back"};
        #endregion

        #region SearchMovieByTitle
        public void SearchByTitle()
        {
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine(Prompt + "\n\n Search Movie by Title");
            ResetColor();
            using (UserMovieDBContext db = new UserMovieDBContext())
            {

                Write("\n Enter Title Of Movie : ");
                string MovieTitle = ReadLine().ToLower().Trim();
                while (MovieTitle == "")
                {
                    Write("\n Enter Valid Movie name : ");
                    MovieTitle = ReadLine();
                }
               
                var MovieGenre = (from m in db.Movies
                                  where m.Title.Contains(MovieTitle)
                                  select new
                                  {
                                      Title = m.Title,
                                      Rating = m.Ratting,
                                      ReleaseYear = m.ReleaseDate.Year,
                                      Genre = m.Genre
                                  }).ToList();
                if (MovieGenre.Count == 0)
                {
                    ForegroundColor = ConsoleColor.Cyan;
                    WriteLine("\n No such Movie found...");
                    ResetColor();
                }
                ForegroundColor = ConsoleColor.Blue;
                WriteLine("\n Movie Title \t\t\tRelease Date \tRating \tGenre");
                ResetColor();
                foreach (var Movie in MovieGenre)
                {
                    Write("\n ----------------------------------------------------------------------\n\n " + Movie.Title.PadRight(32) + "(" + Movie.ReleaseYear + ") \t" + Movie.Rating + " \t");
                    foreach (var genre in Movie.Genre)
                    {
                        Console.Write(genre.GenreName + " \n\t\t\t\t\t\t\t");
                    }

                }
            }
        }
        #endregion

        #region Search By Actor Name
        public void SearchByActor()
        {
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine(Prompt + "\n\n Search Movie by Cast(Actor/Actress)");
            ResetColor();
            using (UserMovieDBContext db = new UserMovieDBContext())
            {
                #region Get Actor List
                List<string> Actors = new List<string>();
                string Choice;
                while (true)
                {
                    Write("\n Enter Actor Name : ");
                    string ActorName = ReadLine().ToLower().Trim();
                    while (!UtilityClass.ValidMovieTitle(ActorName))
                    {
                        Write("\n Enter valid Actor Name : ");
                        ActorName = ReadLine().ToLower().Trim();
                    }
                    Actors.Add(ActorName);
                    Write("\n Do you want to Add another Actor?(y/n) : ");
                    Choice = Console.ReadLine();
                    while (!UtilityClass.ValidChoice(Choice))
                    {
                        Write("\n Please enter valid input:(y/n) : ");
                        Choice = Console.ReadLine();
                    }
                    if (Choice == "n" || Choice == "N")
                    {
                        break;
                    }
                }
                #endregion
                var MovieActorResult = from Movies in db.Movies

                                       select new
                                       {
                                           MovieTitle = Movies.Title,
                                           Rating = Movies.Ratting,
                                           ReleaseYear = Movies.ReleaseDate.Year,
                                           Stars = Movies.StarCasts.ToList()
                                       };
                bool MovieFound = false;
                ForegroundColor = ConsoleColor.Blue;
                WriteLine("\n Movie Title \t\t\tRelease Date \tRating");
                ResetColor();
                foreach (var Movie in MovieActorResult)
                {
                    List<string> ActorsMovie = new List<string>();
                    foreach (var Star in Movie.Stars)
                    {
                        ActorsMovie.Add(Star.ActorName);
                    }
                    var CompareResult = Actors.Except(ActorsMovie).ToList();
                    if (CompareResult.Count == 0)
                    {
                        Write("\n----------------------------------------------------------------------\n\n " + Movie.MovieTitle.PadRight(32) + "(" + Movie.ReleaseYear + ") \t" + Movie.Rating + " \n");
                        MovieFound = true;
                    }
                }
                if (!MovieFound)
                {
                    ForegroundColor = ConsoleColor.Cyan;
                    WriteLine("\n No such Movie found..");
                }
            }
        }
        #endregion

        #region Search By Genre
        public void SearchByGenre()
        {
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine(Prompt + "\n\n Search Movie by Genre");
            ResetColor();
            using (UserMovieDBContext db = new UserMovieDBContext())
            {
                Write("\n Enter Genre : ");
                string GenreName = ReadLine().ToLower().Trim();
                while (string.IsNullOrWhiteSpace(GenreName))
                {
                    Write("\n Enter valid genre name : ");
                    GenreName = ReadLine();
                }
                var MovieGenreResult = (from Movies in db.Movies
                                        where Movies.Genre.Any(g => g.GenreName.Equals(GenreName))
                                        select new
                                        {
                                            MovieTitle = Movies.Title,
                                            Rating = Movies.Ratting,
                                            ReleaseDate = Movies.ReleaseDate,
                                            Genre = Movies.Genre.ToList()
                                        }).ToList();
                if (MovieGenreResult.Count == 0)
                {
                    ForegroundColor = ConsoleColor.Cyan;
                    WriteLine("\n There is no such movies found..");
                    ResetColor();
                    return;
                }

                int Selectedindex;
                do
                {
                    PersonOps PersonOpsObj = new PersonOps();
                    ForegroundColor = ConsoleColor.Yellow;
                    WriteLine(Prompt + "\n\nWelcome to MovieNexx. What Would you like to perform?\n");
                    ResetColor();
                    Menu MainMenu = new Menu(OrderingOptions);
                    Selectedindex = MainMenu.Run();
                    switch (Selectedindex)
                    {
                        case 0:
                            Clear();
                            ForegroundColor = ConsoleColor.Yellow;
                            WriteLine(Prompt + "\n\n Searched Movies by Ascending of Movie Title");
                            ResetColor();
                            var AscByTitle = from Movie in MovieGenreResult
                                             orderby Movie.MovieTitle
                                             select Movie;
                            ForegroundColor = ConsoleColor.Blue;
                            WriteLine("\n Movie Title");
                            ResetColor();
                            foreach (var Movie in AscByTitle)
                            {
                                Console.WriteLine("\n ----------------------------------------\n\n " + Movie.MovieTitle);
                            }
                            ForegroundColor = ConsoleColor.Cyan;
                            WriteLine(" \n Press any key to return back ");
                            ResetColor();
                            ReadLine();
                            break;
                        case 1:
                            Clear();
                            ForegroundColor = ConsoleColor.Yellow;
                            WriteLine(Prompt + "\n\n Searched Movies by Descending of Movie Release Date");
                            ResetColor();
                            var DescByRelease = from Movie in MovieGenreResult
                                                orderby Movie.ReleaseDate descending
                                                select Movie;
                            ForegroundColor = ConsoleColor.Blue;
                            WriteLine("\n Movie Title \t\t\t  Release Date");
                            ResetColor();
                            foreach (var Movie in DescByRelease)
                            {
                                Console.WriteLine("\n -----------------------------------------------------\n\n " + Movie.MovieTitle.PadRight(32) + " " + Movie.ReleaseDate.ToShortDateString());
                            }
                            ForegroundColor = ConsoleColor.Cyan;
                            WriteLine(" \n Press any key to return back ");
                            ResetColor();
                            ReadLine();
                            break;
                        case 2:
                            PersonOpsObj.UserMenu();
                            break;
                    }
                } while(Selectedindex < 3);
                
            }
        }
        #endregion

        #region Search By Duration
        public void SearchByDuration()
        {
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine(Prompt + "\n\n Search Movie by Duration");
            ResetColor();
            using (UserMovieDBContext db = new UserMovieDBContext())
            {
                Write("\n Enter duration in Minutes : ");
                string MinDurationTemp = Console.ReadLine().Trim();
                while (!UtilityClass.ValidMovieDuration(MinDurationTemp))
                {
                    ForegroundColor = ConsoleColor.Red;
                    WriteLine("\n Please enter valid duration");
                    ResetColor();
                    Write("\n Enter minimum duration in Minutes : ");
                    MinDurationTemp = Console.ReadLine().Trim();
                }
                int Min = Convert.ToInt32(MinDurationTemp);

                Write("\n Enter Maximum Duration in min : ");
                string MaxDurationTemp = Console.ReadLine().Trim();
                while (!UtilityClass.ValidMovieDuration(MaxDurationTemp))
                {
                    ForegroundColor = ConsoleColor.Red;
                    WriteLine("\n Please enter valid duration");
                    ResetColor();
                    Write("\n Enter maximum duration in Minutes : ");
                    MaxDurationTemp = Console.ReadLine().Trim();
                }
                int Max = Convert.ToInt32(MaxDurationTemp);

                while (Max < Min)
                {
                    Write("\n Maximum time should be greater than Minimum : ");
                    string MxDurationTemp = Console.ReadLine().Trim();
                    while (!UtilityClass.ValidMovieDuration(MxDurationTemp))
                    {
                        ForegroundColor = ConsoleColor.Red;
                        WriteLine("\n Please enter valid duration");
                        ResetColor();
                        Write("\n Enter maximum duration in Minutes : ");
                        MxDurationTemp = Console.ReadLine().Trim();
                    }
                    Max = Convert.ToInt32(MxDurationTemp);
                }
                var MovieDurationResult = (from Movies in db.Movies
                                           where Movies.Duration >= Min && Movies.Duration <= Max
                                           orderby Movies.Title
                                           select new
                                           {
                                               MovieTitle = Movies.Title,
                                               Rating = Movies.Ratting,
                                               ReleaseDate = Movies.ReleaseDate,
                                               Duration = Movies.Duration
                                           }).ToList();
                if (MovieDurationResult.Count == 0)
                {
                    ForegroundColor = ConsoleColor.Cyan;
                    WriteLine("\n There is no such movies found..");
                    ResetColor();
                    return;
                }
                ForegroundColor = ConsoleColor.Blue;
                WriteLine("\n Movie Title(A-Z) \t\tRating \t\tMovie Duration \t\tRelease Date");
                ResetColor();
                foreach (var Movie in MovieDurationResult)
                {
                    Console.WriteLine("\n ------------------------------------------------------------------------------------\n\n " + Movie.MovieTitle.PadRight(32) + Movie.Rating + " \t\t  " + Movie.Duration + " min \t\t " + Movie.ReleaseDate.ToShortDateString());
                }
            }
        }
        #endregion

        #region Search By Keyword
        public void SearchByKeyword()
        {
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine(Prompt + "\n\n Search Movie by Plot Keywords");
            ResetColor();
            using (UserMovieDBContext db = new UserMovieDBContext())
            {
                Write("\n Enter Keyword : ");
                string Keyword = Console.ReadLine().ToLower().Trim().Trim('#');
                while (string.IsNullOrWhiteSpace(Keyword))
                {
                    Write("\n Enter Valid keyword : ");
                    Keyword = Console.ReadLine().ToLower().Trim().Trim('#');

                }

                var MovieKeywordResult = (from Movies in db.Movies
                                          where Movies.Plot.Contains(Keyword)
                                          orderby Movies.ReleaseDate descending
                                          select new
                                          {
                                              MovieTitle = Movies.Title,
                                              Rating = Movies.Ratting,
                                              ReleaseDate = Movies.ReleaseDate,
                                              Duration = Movies.Duration
                                          }).ToList();
                ForegroundColor = ConsoleColor.Blue;
                WriteLine("\n Movie Title \t\t\tRating \t\tMovie Duration \t\tRelease Date");
                ResetColor();
                if (MovieKeywordResult.Count == 0)
                {
                    ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\n There is no such movie found..");
                    ResetColor();
                    return;
                }
                foreach (var Movie in MovieKeywordResult)
                {
                    Console.WriteLine("\n------------------------------------------------------------------------------------\n\n " + Movie.MovieTitle.PadRight(32) + "" + Movie.Rating + " \t\t " + Movie.Duration + "min \t\t" + Movie.ReleaseDate.ToShortDateString());
                }
            }
        }
        #endregion

        #region TopMovies
        public void TopMovies()
        {
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine(Prompt + "\n\n Top 6 Rated Movie Suggestion for you to Watch");
            ResetColor();
            using (UserMovieDBContext db = new UserMovieDBContext())
            {
                var Result = (from Movies in db.Movies
                              orderby Movies.Ratting descending
                              select Movies).Take(6);
                ForegroundColor = ConsoleColor.Blue;
                WriteLine("\n Movie Title \t\t\t  Rating");
                ResetColor();
                foreach (var result in Result)
                {
                    Console.WriteLine("\n --------------------------------------------------\n\n " + result.Title.PadRight(32) + "  " + result.Ratting);
                }
            }
        }
        #endregion
    }
}
