namespace OwlBank.Models;

public  class UserBuilder
{
    private  readonly User user =  new User();

    public  User Build()
    {
        return user;
    }
    
    public  UserBuilder SetFirstName(string? firstname)
    {
        user.FirstName = firstname;
        return this;
    }
    public  UserBuilder SetLastName(string? lastname){
        user.LastName = lastname;
        return this;
    }

    public  UserBuilder SetEmail(string? email)
    {
        user.Email = email;
        return this;
    }

    public  UserBuilder SetPhoneNumber(string? phoneNumber)
    {
        user.PhoneNumber = phoneNumber;
        return this;
    }

    public  UserBuilder SetDateOfBirth(DateTime? dateOfBirth)
    {
        user.DateOfBirth = dateOfBirth;
        return this;
    }

    public  UserBuilder SetBalance(decimal? balance)
    {
        user.Balance = balance;
        return this;
    }

    public  UserBuilder SetPassword(string? password)
    {
        user.Password = password;
        return this;
    }

    public UserBuilder SetUserName(string? username)
    {
        user.Username = username;
        return this;
    }
}