using DataBase.Model;
using DataBase.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace DataAccessLayer
{
    public class PersonOps
    {

        #region option string
        private readonly string[] MainOptions = { "Login", "Register", "Forgot Password?", "Exit" };
        private readonly string[] UserSearchOptions = { "Search by Title of Movie", "Search by Actor or Actress Name", "Search by Genre", "Search by Duration of Movie", "Search by Plot Keywords", "Suggested Movie (Top 6 Rated Movies)", "Go Back" };
        private readonly string[] AdminSearchOptions = { "Add Movies", "Update Movies", "Delete Movies", "User Management", "Go Back" };
        #endregion

        #region font
        private static readonly string Prompt = @"
███╗   ███╗ ██████╗ ██╗   ██╗██╗███████╗███╗   ██╗███████╗██╗  ██╗
████╗ ████║██╔═══██╗██║   ██║██║██╔════╝████╗  ██║██╔════╝╚██╗██╔╝
██╔████╔██║██║   ██║██║   ██║██║█████╗  ██╔██╗ ██║█████╗   ╚███╔╝   
██║╚██╔╝██║██║   ██║╚██╗ ██╔╝██║██╔══╝  ██║╚██╗██║██╔══╝   ██╔██╗   
██║ ╚═╝ ██║╚██████╔╝ ╚████╔╝ ██║███████╗██║ ╚████║███████╗██╔╝ ██╗
╚═╝     ╚═╝ ╚═════╝   ╚═══╝  ╚═╝╚══════╝╚═╝  ╚═══╝╚══════╝╚═╝  ╚═╝";

        #endregion

        #region start method
        public void Start()
        {
            Title = "MovieNexx";
            RunMainMenu();
            ReadKey(true);
        }
        #endregion

        #region run menu method
        private void RunMainMenu()
        {
            ForegroundColor = ConsoleColor.Yellow;
            ResetColor();
            Menu MainMenu = new Menu(MainOptions);


            ResetColor();
            int Selectedindex = MainMenu.Run();

            switch (Selectedindex)
            {
                case 0:
                    Clear();
                    UserLogin();
                    break;
                case 1:
                    Clear();
                    SignUp();
                    break;
                case 2:
                    Clear();
                    ForgetPassword();
                    break;
                case 3:
                    Exit();
                    break;
            }
        }
        #endregion

        #region User Registration
        public void SignUp()
        {
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine(Prompt + "\n\n Do Register Yourself Here!!!\n");
            ResetColor();
            UserInfo UsersInfoObj = new UserInfo();
            Boolean isRegister = false;
            while (!isRegister)
            {
                Write(" Enter your name : ");
                string Name = ReadLine().ToLower().Trim();
                while (!UtilityClass.ValidName(Name))
                {
                    ForegroundColor = ConsoleColor.Red;
                    WriteLine("\n Please enter valid name");
                    ResetColor();
                    Write("\n Enter your name : ");
                    Name = ReadLine().ToLower().Trim();
                }
                UsersInfoObj.Name = Name;


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
                        if (UtilityClass.isEmailExist(Email))
                        {
                            ForegroundColor = ConsoleColor.Red;
                            WriteLine("\n Email Already Exists..");
                            ResetColor();
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                UsersInfoObj.Email = Email;

                Write("\n Enter your phone number : ");
                string PhoneNo = ReadLine().ToLower().Trim();
                while (!UtilityClass.ValidPhone(PhoneNo))
                {
                    ForegroundColor = ConsoleColor.Red;
                    WriteLine("\n Invalid Phone number");
                    ResetColor();
                    Write("\n Enter your Valid phone number : ");
                    PhoneNo = ReadLine().ToLower().Trim();
                }
                UsersInfoObj.PhoneNumber = PhoneNo;


                Write("\n Enter password(Length (8 - 15) - 1 Uppercase, 1 Lowercase & 1 Digit) : ");
                string Password = UtilityClass.HidePassword();
                while (!UtilityClass.ValidatePassword(Password))
                {
                    ForegroundColor = ConsoleColor.Red;
                    WriteLine("\n\n Invalid password");
                    ResetColor();
                    Write("\n Enter Valid password : ");
                    Password = UtilityClass.HidePassword();
                }
                bool PasswordMatch = false;
                while (!PasswordMatch)
                {
                    WriteLine("\n Confirm password : ");
                    string ConfirmPassword = UtilityClass.HidePassword();
                    if (Password.Equals(ConfirmPassword))
                    {
                        PasswordMatch = true;
                    }
                    else
                    {
                        ForegroundColor = ConsoleColor.Red;
                        WriteLine("\n\n Password doesn't match");
                        ResetColor();
                    }
                }
                UsersInfoObj.Password = UtilityClass.HashPassword(Password);

                Write("\n\n Enter Date of birth(dd/mm/yyyy) : ");
                string Dob = ReadLine().Trim();
                while (!UtilityClass.ValidDate(Dob))
                {
                    ForegroundColor = ConsoleColor.Red;
                    WriteLine("\n Invalid date of birth");
                    ResetColor();
                    Write("\n Enter valid date of birth(dd/mm/yyyy) : ");
                    Dob = ReadLine().Trim();
                }
                UsersInfoObj.DateOfBirth = Dob;

                UsersInfoObj.Role = false;
                
                try
                {
                    using (UserMovieDBContext dBContext = new UserMovieDBContext())
                    {

                        dBContext.Users.Add(UsersInfoObj);
                        dBContext.SaveChanges();
                        ForegroundColor = ConsoleColor.Green;
                        WriteLine("\n Registration successfull");
                        ResetColor();
                        ForegroundColor = ConsoleColor.Cyan;
                        WriteLine("\n Press any key to return back");
                        ResetColor();
                        isRegister = true;
                    }

                }
                catch (EntityException ex)
                {
                    WriteLine("\n You have missed some filed, Try Again !!! " + ex);
                }
            }
        }
        #endregion

        #region User Login
        public void UserLogin()
        {
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine(Prompt + "\n\n Sign-In to MovieNexx ");
            ResetColor();
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

            Write("\n Enter password : ");
            string Password = UtilityClass.HidePassword();
            Console.WriteLine();
            while (!UtilityClass.ValidatePassword(Password))
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine("\n Invalid password");
                ResetColor();
                Write("\n Enter Valid password : ");
                Password = UtilityClass.HidePassword();
                Console.WriteLine();
            }
            string EncryptedPassword = UtilityClass.HashPassword(Password);
            Console.WriteLine();
            using (UserMovieDBContext dBContext = new UserMovieDBContext())
            {
                var Result = (from e in dBContext.Users
                              where e.Email == Email && e.Password == EncryptedPassword
                              select e).FirstOrDefault();

                if (Result != null)
                {
                    ForegroundColor = ConsoleColor.Green;
                    WriteLine("Login successfull");
                    ResetColor();
                    if (Result.Role == false)
                        UserMenu();
                    else
                    {
                        AdminMenu();
                    }
                }
                else
                {
                    ForegroundColor = ConsoleColor.Red;
                    WriteLine("\n Incorrect Email or password, please enter correct password and Email");
                    ResetColor();
                    System.Threading.Thread.Sleep(2000);
                    Clear();
                    UserLogin();
                }
            }
        }
        #endregion

        #region Forgot Password
        public void ForgetPassword()
        {
            string Email;
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine(Prompt + "\n\n Forget Password ");
            ResetColor();
            while (true)
            {
                Write("\n Enter your Email Id : ");
                Email = Console.ReadLine().ToLower().Trim();

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

            string Dob = "";
            if (!string.IsNullOrWhiteSpace(Email))
            {
                Write("\n Enter Date of birth(dd/mm/yyyy) : ");
                Dob = ReadLine().Trim();
                while (!UtilityClass.ValidDate(Dob))
                {
                    ForegroundColor = ConsoleColor.Red;
                    WriteLine("\n Invalid date of birth");
                    ResetColor();
                    Write("\n Enter valid date of birth(dd/mm/yyyy) : ");
                    Dob = ReadLine().Trim();
                }
            }

            using (UserMovieDBContext db = new UserMovieDBContext())
            {
                UserInfo Result = db.Users.Where(user => (user.Email.Equals(Email) && user.DateOfBirth.Equals(Dob))).FirstOrDefault();
                if (Result != null)
                {
                    Write("\n Enter password(Length (8 - 15) - 1 Uppercase, 1 Lowercase & 1 Digit) : ");
                    string Newpassword = UtilityClass.HidePassword();
                    while (!UtilityClass.ValidatePassword(Newpassword))
                    {
                        ForegroundColor = ConsoleColor.Red;
                        WriteLine();
                        WriteLine("\n Invalid password");
                        ResetColor();
                        Write("\n Enter Valid password : ");
                        Newpassword = UtilityClass.HidePassword();
                        WriteLine();
                    }
                    bool PasswordMatch = false;
                    while (!PasswordMatch)
                    {
                        Write("\n Confirm password : ");
                        string NewConfirmPassword = UtilityClass.HidePassword();
                        if (Newpassword.Equals(NewConfirmPassword))
                        {
                            PasswordMatch = true;
                        }
                        else
                        {
                            ForegroundColor = ConsoleColor.Red;
                            WriteLine();
                            WriteLine("\n Password doesn't match");
                            ResetColor();
                        }
                    }
                    Result.Password = UtilityClass.HashPassword(Newpassword);
                    db.SaveChanges();
                    ForegroundColor = ConsoleColor.Green;
                    WriteLine("\n\n Password changed successfully");
                    ResetColor();
                    ForegroundColor = ConsoleColor.Cyan;
                    WriteLine("\n Please press any key to go to main menu ");
                    ReadLine();
                    ResetColor();
                }
                else
                {
                    ForegroundColor = ConsoleColor.Red;
                    WriteLine("\n No user found with this credemtials.");
                    ResetColor();
                    ForegroundColor = ConsoleColor.Cyan;
                    WriteLine("\n Please press any key to re-enter ");
                    ReadLine();
                    ResetColor();
                    Clear();
                    ForgetPassword();
                }
            }

        }
        #endregion

        #region exit method
        private static void Exit()
        {
            WriteLine("Press Any key to exit");
            Environment.Exit(0);
        }
        #endregion

        #region User Menu
        public void UserMenu()
        {
            UserOps UserOpsObj = new UserOps();
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine(Prompt + "\n\nWelcome to MovieNexx. What Would you like to perform?\n");
            ResetColor();
            Menu SearchMenu = new Menu(UserSearchOptions);
            BackgroundColor = ConsoleColor.Magenta;
            ForegroundColor = ConsoleColor.DarkYellow;
            ResetColor();
            int SelectedIndex = SearchMenu.Run();
            switch (SelectedIndex)
            {
                case 0:
                    Clear();
                    UserOpsObj.SearchByTitle();
                    ForegroundColor = ConsoleColor.Cyan;
                    WriteLine(" \n Press any key to return back ");
                    ResetColor();
                    ReadLine();
                    UserMenu();
                    break;
                case 1:
                    Clear();
                    UserOpsObj.SearchByActor();
                    ForegroundColor = ConsoleColor.Cyan;
                    WriteLine(" \n Press any key to return back ");
                    ResetColor();
                    ReadLine();
                    UserMenu();

                    break;
                case 2:
                    Clear();
                    UserOpsObj.SearchByGenre();
                    ForegroundColor = ConsoleColor.Cyan;
                    WriteLine(" \n Press any key to return back ");
                    ResetColor();
                    ReadLine();
                    UserMenu();

                    break;
                case 3:
                    Clear();
                    UserOpsObj.SearchByDuration();
                    ForegroundColor = ConsoleColor.Cyan;
                    WriteLine(" \n Press any key to return back ");
                    ResetColor();
                    ReadLine();
                    UserMenu();
                    break;
                case 4:
                    Clear();
                    UserOpsObj.SearchByKeyword();
                    ForegroundColor = ConsoleColor.Cyan;
                    WriteLine(" \n Press any key to return back ");
                    ResetColor();
                    ReadLine();
                    UserMenu();
                    break;
                case 5:
                    Clear();
                    UserOpsObj.TopMovies();
                    ForegroundColor = ConsoleColor.Cyan;
                    WriteLine(" \n Press any key to return back ");
                    ResetColor();
                    ReadLine();
                    UserMenu();
                    break;
                case 6:
                    RunMainMenu();
                    break;
                default:

                    break;

            }

        }
        #endregion

        #region Admin Menu
        public void AdminMenu()
        {
            int Selectedindex;
            do
            {
                AdminOps AdminOps = new AdminOps();
                Menu MainMenu = new Menu(AdminSearchOptions);
                Selectedindex = MainMenu.Run();

                switch (Selectedindex)
                {
                    case 0:
                        Clear();
                        AdminOps.AddMovie();
                        ForegroundColor = ConsoleColor.Cyan;
                        WriteLine("\n\n Press any key to return back");
                        ReadLine();
                        ResetColor();
                        break;
                    case 1:
                        Clear();
                        AdminOps.UpdateMovie();
                        break;
                    case 2:
                        Clear();
                        AdminOps.DeleteMovie();
                        break;
                    case 3:
                        Clear();
                        AdminOps.UserManagement();
                        break;
                    case 4:
                        RunMainMenu();
                        break;
                }

            } while (Selectedindex < 5);

        }
        #endregion
    }
}
