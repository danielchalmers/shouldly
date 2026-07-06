var homer = new Person { Name = "Homer" };
var marge = new Person { Name = "Marge" };
var bart = new Person { Name = "Bart" };
var lisa = new Person { Name = "Lisa" };
var lineup = new List<Person> { homer, marge, bart, lisa };
lineup.ShouldContainInConsecutiveOrder(new List<Person> { marge, lisa });