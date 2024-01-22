using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

class Program
{
    [Serializable]
    class User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public User()
        {
            Name = "";
            Surname = "";
            Age = 0;
            Phone = "";
            Email = "";
        }
        public User(string name, string surname, int age, string phone, string email)
        {
            Name = name;
            Surname = surname;
            Age = age;
            Phone = phone;
            Email = email;
        }
        public virtual string ToString() => $"Name :: {Name}\nSurname :: {Surname}\nAge :: {Age}\nPhone :: {Phone}\nEmail :: {Email}";
    }

    [Serializable]
    class UserManagement
    {
        public List<User> Users { get; set; }
        public BinaryFormatter formatter { get; set; }
        public UserManagement()
        {
            Users = new List<User>();
            formatter = new BinaryFormatter();
            Load();
        }
        public void Update(int index, User NewUser)
        {
            for (int i = 0; i < Users.Count; i++)
            {
                if (i == index)
                {
                    Users[i] = NewUser;
                    break;
                }
            }
        }
        public void AddUser(User u)
        {
            Users.Add(u);
        }
        public void RemoveUser(int i)
        {
            Users.RemoveAt(i);
        }
        public void AllShow()
        {
            for (int i = 0; i < Users.Count; i++)
            {
                Console.WriteLine($"#{i}\n"+Users[i].ToString());
            }
        }
        public void ShowUser(string p)
        {
            foreach (var u in Users)
            {
                if (u.Email == p || u.Name == p)
                {
                    Console.WriteLine(u.ToString());
                    return;
                }
            }
            Console.WriteLine("Not found");
        }
        public void Load()
        {
            if (File.Exists("Users.bin"))
            {
                using (Stream fstream = File.OpenRead("Users.bin"))
                {
                    Users = (List<User>)formatter.Deserialize(fstream);
                }
            }
        }
        public void Save()
        {
            using (Stream fstream = File.Create("Users.bin"))
            {
                formatter.Serialize(fstream, Users);
            }
        }
        public void Menu()
        {
            while (true)
            {

                Console.Clear();
                Console.WriteLine("User Management\n1 - Add User\n2 - RemoveUser\n3 - Show users\n4 - Show user(Email)\n5 - Show user(Name)\n6 - Update user\n0 - Exit\nChoice :: ");
                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 0:
                        {
                            Console.WriteLine("Good day! ;)");
                            Thread.Sleep(1000);
                            Save();
                            return;
                        }
                    case 1:
                        {
                            User NewUser = new User();
                            string tmp;
                            //Name
                            Console.WriteLine("Enter name :: ");
                            tmp = Console.ReadLine();
                            if (!Regex.IsMatch(tmp, @"^[A-Za-z]{3,20$}")) NewUser.Name = tmp;
                            else
                            {
                                Console.WriteLine("ERROR");
                                Thread.Sleep(1000);
                                break;
                            }
                            //Surname
                            Console.WriteLine("Enter Surname :: ");
                            tmp = Console.ReadLine();
                            if (!Regex.IsMatch(tmp, @"^[a-zA-Z]{3,20$}")) NewUser.Surname = tmp;
                            else
                            {
                                Console.WriteLine("ERROR");
                                Thread.Sleep(1000);
                                break;
                            }
                            //Age
                            Console.WriteLine("Enter Age :: ");
                            tmp = Console.ReadLine();
                            if (int.Parse(tmp) >= 18) NewUser.Age = int.Parse(tmp);
                            else
                            {
                                Console.WriteLine("ERROR");
                                Thread.Sleep(1000);
                                break;
                            }
                            //Phone
                            Console.WriteLine("Enter Phone :: ");
                            tmp = Console.ReadLine();
                            if (Regex.IsMatch(tmp, @"^\+\d{10}")) NewUser.Phone = tmp;
                            else
                            {
                                Console.WriteLine("ERROR");
                                Thread.Sleep(1000);
                                break;
                            }
                            //Email
                            Console.WriteLine("Enter Email :: ");
                            tmp = Console.ReadLine();
                            if (Regex.IsMatch(tmp, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")) {
                                foreach (var u in Users)
                                {
                                    if (u.Email == tmp)
                                    {
                                        Console.WriteLine("ERROR");
                                        Thread.Sleep(1000);
                                        NewUser = null;
                                        break;
                                    }                           
                                }
                            }
                            else
                            {
                                Console.WriteLine("ERROR");
                                Thread.Sleep(1000);
                                break;
                            }
                            if (NewUser != null)
                            {
                                NewUser.Email = tmp;
                                Users.Add(NewUser);
                                Console.WriteLine("User Added!!! :)");
                            }
                            Thread.Sleep(1000);
                            break;
                        }
                    case 2:
                        {
                            if (Users.Count == 0)
                            {
                                Console.WriteLine("Empty");
                                Thread.Sleep(1000);
                                return;
                            }
                            Console.WriteLine($"Enter index(0-{Users.Count-1}) :: ");
                            int index = int.Parse(Console.ReadLine());
                            RemoveUser(index);
                            Console.WriteLine("Remove");
                            Thread.Sleep(1000);
                            break;
                        }
                    case 3:
                        {
                            AllShow();
                            Console.ReadKey();
                            break;
                        }
                    case 4:
                        {
                            Console.WriteLine("Enter Email :: ");
                            string email = Console.ReadLine();
                            ShowUser(email);
                            Console.ReadKey();
                            break;
                        }
                    case 5:
                        {
                            Console.WriteLine("Enter Name :: ");
                            string name = Console.ReadLine();
                            ShowUser(name);
                            Console.ReadKey();
                            break;
                        }
                    case 6:
                        {
                            if (Users.Count == 0)
                            {
                                Console.WriteLine("Empty");
                                Thread.Sleep(1000);
                                return;
                            }
                            Console.WriteLine($"Enter index(0-{Users.Count-1}) :: ");
                            int index = int.Parse(Console.ReadLine());
                            if (index < Users.Count && index >= 0)
                            {
                                User NewUser = new User();
                                string tmp;
                                //Name
                                Console.WriteLine("Enter name :: ");
                                tmp = Console.ReadLine();
                                if (!Regex.IsMatch(tmp, @"^[A-Za-z]{3,20$}")) NewUser.Name = tmp;
                                else
                                {
                                    Console.WriteLine("ERROR");
                                    Thread.Sleep(1000);
                                    break;
                                }
                                //Surname
                                Console.WriteLine("Enter Surname :: ");
                                tmp = Console.ReadLine();
                                if (!Regex.IsMatch(tmp, @"^[a-zA-Z]{3,20$}")) NewUser.Surname = tmp;
                                else
                                {
                                    Console.WriteLine("ERROR");
                                    Thread.Sleep(1000);
                                    break;
                                }
                                //Age
                                Console.WriteLine("Enter Age :: ");
                                tmp = Console.ReadLine();
                                if (int.Parse(tmp) >= 18) NewUser.Age = int.Parse(tmp);
                                else
                                {
                                    Console.WriteLine("ERROR");
                                    Thread.Sleep(1000);
                                    break;
                                }
                                //Phone
                                Console.WriteLine("Enter Phone :: ");
                                tmp = Console.ReadLine();
                                if (Regex.IsMatch(tmp, @"^\+\d{10}")) NewUser.Phone = tmp;
                                else
                                {
                                    Console.WriteLine("ERROR");
                                    Thread.Sleep(1000);
                                    break;
                                }
                                //Email
                                Console.WriteLine("Enter Email :: ");
                                tmp = Console.ReadLine();
                                if (Regex.IsMatch(tmp, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                                {
                                    foreach (var u in Users)
                                    {
                                        if (u.Email == tmp)
                                        {
                                            Console.WriteLine("ERROR");
                                            Thread.Sleep(1000);
                                            NewUser = null;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("ERROR");
                                    Thread.Sleep(1000);
                                    break;
                                }
                                if (NewUser != null)
                                {
                                    NewUser.Email = tmp; 
                                    Update(index, NewUser);
                                }
                            }

                            break;
                        }
                }
            }
        }
    }
    static void Main()
    {
        UserManagement userManagement = new UserManagement();
        userManagement.Menu();
    }
}