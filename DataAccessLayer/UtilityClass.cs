using DataBase.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public static class UtilityClass
    {
        #region Hide Password
        public static string HidePassword()
        {
            string Password = "";
            ConsoleKeyInfo Info = Console.ReadKey(true);
            while (Info.Key != ConsoleKey.Enter)
            {
                if (Info.Key != ConsoleKey.Backspace)
                {
                    Console.Write("*");
                    Password += Info.KeyChar;
                }
                else if (Info.Key == ConsoleKey.Backspace)
                {
                    if (!string.IsNullOrEmpty(Password))
                    {

                        Password = Password.Substring(0, Password.Length - 1);

                        int Pos = Console.CursorLeft;

                        Console.SetCursorPosition(Pos - 1, Console.CursorTop);

                        Console.Write(" ");

                        Console.SetCursorPosition(Pos - 1, Console.CursorTop);
                    }
                }
                Info = Console.ReadKey(true);
            }


            return Password;

        }
        #endregion
        #region Validate Password
        public static Boolean ValidatePassword(string Password)
        {
            const int MIN_LENGTH = 8;
            const int MAX_LENGTH = 15;

            if (string.IsNullOrWhiteSpace(Password))
                return false;

            bool MeetsLengthRequirements = Password.Length >= MIN_LENGTH && Password.Length <= MAX_LENGTH;
            bool HasUpperCaseLetter = false;
            bool HasLowerCaseLetter = false;
            bool HasDecimalDigit = false;

            if (MeetsLengthRequirements)
            {
                foreach (char c in Password)
                {
                    if (char.IsUpper(c)) HasUpperCaseLetter = true;
                    else if (char.IsLower(c)) HasLowerCaseLetter = true;
                    else if (char.IsDigit(c)) HasDecimalDigit = true;
                }
            }

            bool isValid = MeetsLengthRequirements
                        && HasUpperCaseLetter
                        && HasLowerCaseLetter
                        && HasDecimalDigit
                        ;
            return isValid;

        }

        
        #endregion
        #region ValidEmail
        public static Boolean ValidEmail(string Email)
        {
            if (string.IsNullOrWhiteSpace(Email))
                return false;

            bool isValid = false;
            Regex Regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match Match = Regex.Match(Email);
            if (Match.Success)
                isValid = true;

            return isValid;

        }
        #endregion
        #region Valid Phone
        public static Boolean ValidPhone(string Phone)
        {
            if (string.IsNullOrWhiteSpace(Phone))
                return false;

            bool isValid = false;
            Regex Regex = new Regex(@"^[6-9]{1}[0-9]{9}$");
            Match Match = Regex.Match(Phone);
            if (Match.Success)
                isValid = true;

            return isValid;

        }
        #endregion
        #region ValidName

        public static Boolean ValidName(string Name)
        {
            const int MIN_LENGTH = 2;
            const int MAX_LENGTH = 40;
            if (string.IsNullOrWhiteSpace(Name) || Name.Length <= MIN_LENGTH || Name.Length >= MAX_LENGTH )
                return false;

            return true;
        }
        #endregion
        #region Valid Date
        public static Boolean ValidDate(string Dob)
        {
            if (string.IsNullOrWhiteSpace(Dob))
                return false;

            bool isValidFormat = false;
            bool isLessThanCurrentDate = false;
            string Pattern = "dd/MM/yyyy";
            DateTime ParsedDate;
            if (DateTime.TryParseExact(Dob, Pattern, null, System.Globalization.DateTimeStyles.None, out ParsedDate))
            {
                isValidFormat = true;
            }

            var TodaysDate = DateTime.Today;

            if (ParsedDate < TodaysDate)
            {
                isLessThanCurrentDate = true;
            }
            return isValidFormat && isLessThanCurrentDate;

        }
        #endregion
        #region Hash Password
        public static string HashPassword(string Password)
        {
            var Provider = new SHA1CryptoServiceProvider();
            var Encoding = new UnicodeEncoding();
            byte[] Encrypted = Provider.ComputeHash(Encoding.GetBytes(Password));
            return Convert.ToBase64String(Encrypted);
        }
        #endregion
        #region Email Exist or Not
        public static Boolean isEmailExist(string Email)
        {
            bool isValid = false;
            using (UserMovieDBContext db = new UserMovieDBContext())
            {
                var Result = db.Users.Where(u => u.Email.Equals(Email)).ToList();
                if (Result.Count != 0)
                {
                    isValid = true;
                }
            }

            return isValid;
        }
        #endregion
        #region ValidMovieTitle

        public static Boolean ValidMovieTitle(String Title)
        {
            const int MIN_LENGTH = 2;
            const int MAX_LENGTH = 100;
            if (string.IsNullOrWhiteSpace(Title) || Title.Length <= MIN_LENGTH || Title.Length >= MAX_LENGTH)
                return false;

            return true;
        }
        #endregion
        #region ValidMovieDuration
        public static Boolean ValidMovieDuration(string Duration)
        {
            int Output;
            if (!int.TryParse(Duration, out Output))
                return false;
           
            const int MIN_DURATION = 1;
            const int MAX_DURATION = 500;
            if (Output < MIN_DURATION || Output > MAX_DURATION)
                return false;

            return true;
        }
        #endregion
        #region ValidRating
        public static bool ValidRating(string Rating)
        {
            float Output;
            if (!float.TryParse(Rating, out Output))
                return false;

            const float MIN_RATING = 1.0f;
            const float MAX_RATING = 10.0f;
            if (Output < MIN_RATING || Output > MAX_RATING)
                return false;

            return true;
        }
        #endregion
        #region ValidChoice

        public static Boolean ValidChoice(string Choice)
        {
            if (Choice.ToLower().Trim() == "y" || Choice.ToLower().Trim() == "n")
                return true;

            return false;
        }
        #endregion
    }
}
