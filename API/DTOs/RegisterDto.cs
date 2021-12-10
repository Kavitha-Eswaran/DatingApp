using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        // We can use our own casing for the properties defined in DTO.
        // It is our personal preference to have the properties in this casing, 
        // as we have the same properties defined in our AppUser entity with camel case.
        //The attribute [APIController]’s another feature is to automatically validate 
        //the parameters that we pass to the API controller/we pass in API endpoint, based on the validation we set.
        //As we are passing the DTO object to the API controller, we are able to apply the validation on the DTO properties as below. 

        //Annotations:
        //[RegularExpression]--> to validate the value matches the sepcifed regular expression.
        //[EmailAddress] --> to validate the value matches the email address pattern.
        //[Phone] --> to ensure the data matches the well-formed phone number.
        //[StringLength] --> to specifiy the minimum and maximum length of characters
        // that are allowed in the data field.
        //[Required] --> to make the field value mandatory.
        [Required]        
        public string Username { get; set; }

        [Required]
        [StringLength(8, MinimumLength =4)]
        public string Password { get; set; }
    }
}