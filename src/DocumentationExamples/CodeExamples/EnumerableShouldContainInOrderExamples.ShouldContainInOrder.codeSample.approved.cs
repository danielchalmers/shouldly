var homer = new Person { Name = "Homer" };
var moe = new Person { Name = "Moe" };
var barney = new Person { Name = "Barney" };
var arrivals = new List<Person> { moe, homer, barney };
arrivals.ShouldContainInOrder(new List<Person> { homer, moe });