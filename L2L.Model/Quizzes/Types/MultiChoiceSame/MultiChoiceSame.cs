using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2L.Entities
{
    public class MultiChoiceSameQuestion : QzEditor, IQuestion
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answers { get; set; }
        public bool IsMultiplePoints { get; set; }

        // Foreign Keys
        public int TestId { get; set; }
        public int ChoiceGroupId { get; set; }

        // Navigation Properties
        public virtual Test Test { get; set; }
        public virtual MultiChoiceSameChoiceGroup ChoiceGroup { get; set; }
    }

    public class MultiChoiceSameChoiceGroup
    {
        public MultiChoiceSameChoiceGroup()
        {
            Choices = new List<MultiChoiceSameChoice>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool ShuffleChoices { get; set; }
        public bool IsMultiplePoints { get; set; }

        // Foreign Keys
        public int TestId { get; set; }

        // Navigation Properties
        public virtual Test Test { get; set; }
        public virtual IList<MultiChoiceSameChoice> Choices { get; set; }
        public virtual IList<MultiChoiceSameQuestion> Questions { get; set; }
    }

    public class MultiChoiceSameChoice 
    {
        public int Id { get; set; }
        public string Value { get; set; }

        // Foreign Keys
        public int ChoiceGroupId { get; set; }

        // Navigation Properties
        public virtual MultiChoiceSameChoiceGroup ChoiceGroup { get; set; }
    }
}
