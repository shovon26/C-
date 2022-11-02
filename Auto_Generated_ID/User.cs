// See https://aka.ms/new-console-template for more information
using System.Data;
using System.Reflection.Metadata.Ecma335;

public class User
{
    static int increment = 1;
    private int _ID;
    private string _Name;
    private string _DOB;
    private string _Email;
    private string _Phone;

    public User()
    {

    }
    public User(string name, string dOB, string email, string phone)
    {
        _ID = User.increment++;
        _Name = name;
        _DOB = dOB;
        _Email = email;
        _Phone = phone;
    }

    public User(User user)
    {
        this._ID = user._ID;
        this._Name = user._Name;
        this._DOB = user._DOB;
        this._Email = user._Email;
        this._Phone = user._Phone;
    }

    public int ID
    {
        get { return _ID; }
    }

    public string Name
    {
        get { return _Name; }
    }

    public string DOB
    {
        get { return _DOB; }
    }

    public string Email
    {
        get { return _Email; }
    }

    public string Phone
    {
        get { return _Phone; }
    }

    public void getAge(string date)
    {
        int curDate = ConvertInvoiceDate();
        int birthDate = ConvertBirthdayDate(date);

        Tuple<int, int, int> T1 = findYearMonthDay(curDate);
        Tuple<int, int, int> T2 = findYearMonthDay(birthDate);

        findAge(T1.Item1, T1.Item2, T1.Item3, T2.Item1, T2.Item2, T2.Item3);

    }



    public void userInformation()
    {
        Console.WriteLine("User Information of ID : " + ID);
        Console.WriteLine("Name : " + Name + ", DOB : " + DOB + ", Email : " + Email + ", Phone : " + Phone);
    }

    private int ConvertInvoiceDate()
    {
        return int.Parse(DateTime.Today.ToString("yyyyMMdd"));
    }

    private int ConvertBirthdayDate(string s)
    {
        return int.Parse(DateTime.Today.ToString(s));
    }
    private void findAge(int current_date, int current_month, int current_year, int birth_date, int birth_month, int birth_year)
    {
        int[] month = { 31, 28, 31, 30, 31, 30,
                    31, 31, 30, 31, 30, 31 };

        if (birth_date > current_date)
        {
            current_date
                = current_date + month[birth_month - 1];
            current_month = current_month - 1;
        }

        if (birth_month > current_month)
        {
            current_year = current_year - 1;
            current_month = current_month + 12;
        }

        int calculated_date = current_date - birth_date;
        int calculated_month = current_month - birth_month;
        int calculated_year = current_year - birth_year;

        Console.WriteLine("Age is : " + calculated_year + " Year, " + calculated_month + " month, " + calculated_date + " day");
    }

    private Tuple<int, int, int> findYearMonthDay(int date)
    {
        string tmp = "";
        while (date > 0)
        {
            tmp += (date % 10).ToString();
            date /= 10;
        }
        int m = 1;
        int yr = 0;
        for(int i=4; i<tmp.Length; i++)
        {
            yr += m * (tmp[i] - '0');
            m *= 10;
        }
        m = 1;
        int mn = 0;
        for(int i=2; i<=3; i++)
        {
            mn += m * (tmp[i] - '0');
            m *= 10;
        }
        m = 1;
        int dd = 0;
        for(int i=0; i<=1; i++)
        {
            dd += m * (tmp[i] - '0');
            m *= 10;
        }
        Tuple<int, int, int> tple = new Tuple<int, int, int>(dd, mn, yr);
        return tple;
    }
   
}
