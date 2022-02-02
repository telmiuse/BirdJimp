using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class RegisterManager : MonoBehaviour
{
    [SerializeField] InputField PasswordLoginText;
    [SerializeField] InputField EmailLoginText;
    [SerializeField] InputField NameReggisterText;
    [SerializeField] InputField EmailRegisterText;
    [SerializeField] InputField PasswordRegisterText;

    [SerializeField] Text PasswordLoginTextError;
    [SerializeField] Text EmailLoginTextError;
    [SerializeField] Text NameReggisterTextError;
    [SerializeField] Text EmailRegisterTextError;
    [SerializeField] Text PasswordRegisterTextError;

    private string Pass;
    private string Email;
    private string Name;

    public void Login()
    {
        PasswordLoginTextError.text = "";
        EmailLoginTextError.text = "";
        EmailLoginText.image.color = Color.white;
        PasswordLoginText.image.color = Color.white;
        bool ok = true;
        Pass = PasswordLoginText.text;
        Email = EmailLoginText.text;
        PlayerPrefs.SetString("Email", Email);
        PlayerPrefs.SetString("Pass", Pass);
        if (!IsValidEmail(Email)) {
            WrongLogMail();
            ok = false;
        }
        if(Pass.Length <= 6)
        {
            WrongLogPass();
            ok = false;
        }

        if(ok) PlayFabLog.Login(Email, Pass);
    }

    public void Start()
    {
        if (PlayerPrefs.GetString("Email") != "") EmailLoginText.text = PlayerPrefs.GetString("Email");
        if (PlayerPrefs.GetString("Pass")  != "") PasswordLoginText.text = PlayerPrefs.GetString("Pass");
    }
    public void WrongLogMail()
    {
        EmailLoginText.image.color = Color.red;
        EmailLoginTextError.text = "Bad Mail Format";
    }
    public void WrongLogPass()
    {
        PasswordLoginText.image.color = Color.red;
        PasswordLoginTextError.text = "6 char min";
    }
    public void WrongRegMail()
    {
        EmailRegisterText.image.color = Color.red;
        EmailRegisterTextError.text = "Bad Mail Format";
    }
    public void WrongRegPass()
    {
        PasswordRegisterText.image.color = Color.red;
        PasswordRegisterTextError.text = "6 char min";
    }
    public void WrongRegName()
    {
        NameReggisterText.image.color = Color.red;
        NameReggisterTextError.text = "3 char min";
    }
    public void Register()
    {
        NameReggisterTextError.text = "";
        EmailRegisterTextError.text="";
        PasswordRegisterTextError.text="";
        EmailRegisterText.image.color = Color.white;
        PasswordRegisterText.image.color = Color.white;
        NameReggisterText.image.color = Color.white;
        bool ok = true;
        Pass = PasswordRegisterText.text;
        Name = NameReggisterText.text;
        Email = EmailRegisterText.text;
        PlayerPrefs.SetString("Email", Email);
        PlayerPrefs.SetString("Pass", Pass);

        if (Name.Length < 2)
        {
            WrongRegName();
            ok = false;
        }
        if (!IsValidEmail(Email))
        {
            WrongRegMail();
            ok = false;
        }
        if (Pass.Length <= 6)
        {
            WrongRegPass();
            ok = false;
        }

        if(ok) PlayFabLog.Register(Name, Email, Pass);
    }
    public void LoginRequest()
    {

    }

    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            // Normalize the domain
            email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                  RegexOptions.None, TimeSpan.FromMilliseconds(200));

            // Examines the domain part of the email and normalizes it.
            string DomainMapper(Match match)
            {
                // Use IdnMapping class to convert Unicode domain names.
                var idn = new IdnMapping();

                // Pull out and process domain name (throws ArgumentException on invalid)
                string domainName = idn.GetAscii(match.Groups[2].Value);

                return match.Groups[1].Value + domainName;
            }
        }
        catch (RegexMatchTimeoutException e)
        {
            return false;
        }
        catch (ArgumentException e)
        {
            return false;
        }

        try
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
}
