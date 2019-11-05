using Core.Entities.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Services.Validation
{
    public class EntityValidator<TEntity> where TEntity : class
    {
        static IEnumerable<ValidationResult> Validate(TEntity entity)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(entity, null, null);
            Validator.TryValidateObject(entity, validationContext, validationResults, true);
            return validationResults;
        }

        public static IEnumerable<MessageInfo> ValidateEntity(TEntity entity)
        {
            var results = Validate(entity);

            var errorMessages = new List<MessageInfo>();

            if (results.Any())
            {
                foreach (var result in results)
                {
                    errorMessages.Add(new MessageInfo
                    {
                        MessageInfoType = MessageInfoType.Error,
                        Message = result.ErrorMessage,
                        Field = result.MemberNames.ToList().Count > 0 
                            ? result.MemberNames.ToList()[0] 
                            : string.Empty
                    });
                }
            }

            return errorMessages;
        }
    }
}
