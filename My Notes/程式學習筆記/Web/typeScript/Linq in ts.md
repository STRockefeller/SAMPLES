# TypeScript vs. C#: LINQ

[reference](https://decembersoft.com/posts/typescript-vs-csharp-linq/)

C# LinQ 真的把我寵壞了

---

last updated: Nov 14th, 2017

[TypeScript](https://decembersoft.com/categories/typescript/)

TypeScript has no equivalent for the language-integrated-natural-query aspect of LINQ. (hey, isn't that *literally* the whole acronym?)

True, you *can't* write the following LINQ statement in TypeScript

```cs
var adultUserNames =  from u in users  where u.Age >= 18  select u.Name;
```

However, the `IEnumerable<T>` extension methods, which are at the heart of LINQ, have equivalents in TypeScript (or can be emulated).

- [Aggregate](https://decembersoft.com/posts/typescript-vs-csharp-linq/#aggregate)
- [All](https://decembersoft.com/posts/typescript-vs-csharp-linq/#all)
- [Any](https://decembersoft.com/posts/typescript-vs-csharp-linq/#any)
- [Append](https://decembersoft.com/posts/typescript-vs-csharp-linq/#append)
- [Average](https://decembersoft.com/posts/typescript-vs-csharp-linq/#average)
- [Cast](https://decembersoft.com/posts/typescript-vs-csharp-linq/#cast)
- [Concat](https://decembersoft.com/posts/typescript-vs-csharp-linq/#concat)
- [Contains](https://decembersoft.com/posts/typescript-vs-csharp-linq/#contains)
- [Count](https://decembersoft.com/posts/typescript-vs-csharp-linq/#count)
- [DefaultIfEmpty](https://decembersoft.com/posts/typescript-vs-csharp-linq/#defaultifempty)
- [Distinct](https://decembersoft.com/posts/typescript-vs-csharp-linq/#distinct)
- [ElementAt](https://decembersoft.com/posts/typescript-vs-csharp-linq/#elementat)
- [ElementAtOrDefault](https://decembersoft.com/posts/typescript-vs-csharp-linq/#elementatordefault)
- [Empty](https://decembersoft.com/posts/typescript-vs-csharp-linq/#empty)
- [Except](https://decembersoft.com/posts/typescript-vs-csharp-linq/#except)
- [First](https://decembersoft.com/posts/typescript-vs-csharp-linq/#first)
- [FirstOrDefault](https://decembersoft.com/posts/typescript-vs-csharp-linq/#firstordefault)
- [List.ForEach](https://decembersoft.com/posts/typescript-vs-csharp-linq/#list-foreach)
- [GroupBy](https://decembersoft.com/posts/typescript-vs-csharp-linq/#groupby)
- [Intersect](https://decembersoft.com/posts/typescript-vs-csharp-linq/#intersect)
- [Last](https://decembersoft.com/posts/typescript-vs-csharp-linq/#last)
- [LastOrDefault](https://decembersoft.com/posts/typescript-vs-csharp-linq/#lastordefault)
- [Max](https://decembersoft.com/posts/typescript-vs-csharp-linq/#max)
- [Min](https://decembersoft.com/posts/typescript-vs-csharp-linq/#min)
- [OfType](https://decembersoft.com/posts/typescript-vs-csharp-linq/#oftype)
- [OrderBy / ThenBy](https://decembersoft.com/posts/typescript-vs-csharp-linq/#orderby-thenby)
- [Reverse](https://decembersoft.com/posts/typescript-vs-csharp-linq/#reverse)
- [Select](https://decembersoft.com/posts/typescript-vs-csharp-linq/#select)
- [SelectMany](https://decembersoft.com/posts/typescript-vs-csharp-linq/#selectmany)
- [Single](https://decembersoft.com/posts/typescript-vs-csharp-linq/#single)
- [SingleOrDefault](https://decembersoft.com/posts/typescript-vs-csharp-linq/#singleordefault)
- [Skip](https://decembersoft.com/posts/typescript-vs-csharp-linq/#skip)
- [SkipWhile](https://decembersoft.com/posts/typescript-vs-csharp-linq/#skipwhile)
- [Sum](https://decembersoft.com/posts/typescript-vs-csharp-linq/#sum)
- [Take](https://decembersoft.com/posts/typescript-vs-csharp-linq/#take)
- [TakeWhile](https://decembersoft.com/posts/typescript-vs-csharp-linq/#takewhile)
- [Union](https://decembersoft.com/posts/typescript-vs-csharp-linq/#union)
- [Where](https://decembersoft.com/posts/typescript-vs-csharp-linq/#where)
- [Zip](https://decembersoft.com/posts/typescript-vs-csharp-linq/#zip)

### Aggregate

```cs
var leftToRight = users.Aggregate(initialValue, (a, u) => /* ... */);
```
```TypeScript
const leftToRight = users.reduce((a, u) => /* ... */, initialValue);
const rightToLeft = users.reduceRight((a, u) => /* ... */, initialValue);
```

### All

```cs
var allReady = users.All(u => u.IsReady);
```
```TypeScript
const allReady = users.every(u => u.isReady);
```

### Any

```cs
var isDirty = users.Any(u => u.IsDirty);
```
```TypeScript
const isDirty = users.some(u => u.isDirty);
```

### Append

```cs
var allUsers = users.Append(oneMoreUser);
```
```TypeScript
const allUsers = [ ...users, oneMoreUser ];
```

### Average

```cs
var avgAge = users.Average(u => u.Age);
```
```TypeScript
if (users.length < 1) {  throw new Error('source contains no elements');}const avgAge = users.reduce((a, u) => a + u.age, 0) / users.length;
```

### Cast

```cs
var people = users.Cast<Person>();
```
```TypeScript
const people = users as Person[];// Note: not semantically the same. The C# version throws an exception// if any of the users can't be cast to type Person.
```

### Concat

```cs
var allUsers = users.Concat(moreUsers);
```
```TypeScript
const allUsers = [ ...users, ...moreUsers ];
```

### Contains

```cs
var hasAdmin = users.Contains(admin);
```
```TypeScript
const hasAdmin = users.includes(admin); // Use a polyfill for IE support
```

### Count

```cs
var n = users.Count();
```
```TypeScript
const n = users.length;
```

### DefaultIfEmpty

```cs
var nonEmptyUsers = Enumerable.DefaultIfEmpty(users);
```
```TypeScript
const nonEmptyUsers = users.length ? users : [ null ];
```

### Distinct

```cs
var uniqueNames = users.Select(u => u.Name).Distinct();
```
```TypeScript
const uniqueNames = Object.keys(  users.map(u => u.name).reduce(    (un, u) => ({ ...un, n }),    {}  ));
```

### ElementAt

```cs
var nth = users.ElementAt(n);
```
```TypeScript
if (n < 0 || n > users.length) {  throw new Error('Index was out of range');}const nth = users[n];
```

### ElementAtOrDefault

```cs
var nth = users.ElementAtOrDefault(n);
```
```TypeScript
const nth = users[n];
```

### Empty

```cs
var noUsers = IEnumerable.Empty<User>();
```
```TypeScript
const noUsers: User[] = [];const noUsers = [] as User[];
```

### Except

```cs
var maleUsers = users.Except(femaleUsers);
```
```TypeScript
const maleUsers = users.filter(u =>  !femaleUsers.includes(u) // Use a polyfill for IE support);
```

### First

```cs
var first = users.First();
```
```TypeScript
if (users.length < 1) {  throw new Error('Sequence contains no elements');}const first = users[0];
```

### FirstOrDefault

```cs
var first = users.FirstOrDefault();
```
```TypeScript
const first = users[0];
```

### List.ForEach

```cs
users.ToList().ForEach(u => /* ... */);
```
```TypeScript
users.forEach(u => /* ... */);
```

### GroupBy

```cs
var usersByCountry = users.GroupBy(u => u.Country);
```
```TypeScript
const usersByCountry = users.reduce((ubc, u) => ({  ...ubc,  [u.country]: [ ...(ubc[u.country] || []), u ],}), {});
```

### Intersect

```cs
var targetUsers = usersWhoClicked.Intersect(usersBetween25And45);
```
```TypeScript
const targetUsers = usersWhoClicked.filter(u =>  usersBetween25And45.includes(u) // Use a polyfill for IE support);
```

### Last

```cs
var last = users.Last();
```
```TypeScript
if (users.length < 1) {  throw new Error('Sequence contains no elements');}const last = users[users.length - 1];
```

### LastOrDefault

```cs
var last = users.LastOrDefault();
```
```TypeScript
const last = users[users.length - 1];
```

### Max

```cs
var oldestAge = users.Max(u => u.Age);
```
```TypeScript
if (users.length < 1) {  throw new Error('source contains no elements');}const oldestAge = users.reduce((oa, u) => Math.max(oa, u.age), 0);
```

### Min

```cs
var youngestAge = users.Min(u => u.Age);
```
```TypeScript
if (users.length < 1) {  throw new Error('source contains no elements');}const youngestAge = users.reduce((ya, u) => Math.min(ya, u.age), Number.MAX_VALUE);
```

### OfType

```cs
var bots = users.OfType<Bot>();
```
```TypeScript
// TypeScript// No equivalent
```

### OrderBy / ThenBy

```cs
var sorted = users.OrderBy(u => u.Age).ThenBy(u => u.Name);
```
```TypeScript
const sorted = users.sort((a, b) => {  const ageDiff = b.age - a.age;  if (ageDiff) return ageDiff;  return a.name.localeCompare(b.name); // Use a polyfill for IE support});
```

### Reverse

```cs
var backwards = users.Reverse();
```
```TypeScript
const backwards = users.reverse();// Caution: users is also reversed!
```

### Select

```cs
var names = users.Select(u => u.Name);
```
```TypeScript
const names = users.map(u => u.name);
```

### SelectMany

```cs
var phoneNumbers = users.SelectMany(u => u.PhoneNumbers);
```
```TypeScript
const phoneNumbers = users.reduce((pn, u) => [ ...pn, ...u.phoneNumbers ], []);
```

### Single

```cs
var user = users.Single();
```
```TypeScript
if (users.length > 1) {  throw new Error('The input sequence contains more than one element');}else if (!users.length) {  throw new Error('The input sequence is empty');}const user = users[0];
```

### SingleOrDefault

```cs
var user = users.Single();
```
```TypeScript
const user = users[0];
```

### Skip

```cs
var otherUsers = users.Skip(n);
```
```TypeScript
const otherUsers = users.filter((u, i) => i >= n);
```

### SkipWhile

```cs
var otherUsers = users.SkipWhile(predicate);
```
```TypeScript
let i = 0;while (i < users.length && predicate(users[i++]));const otherUsers = users.slice(i - 1);
```

### Sum

```cs
var totalYears = users.Sum(u => u.Age);
```
```TypeScript
if (users.length < 1) {  throw new Error('source contains no elements');}const totalYears = users.reduce((ty, u) => ty + u, 0);
```

### Take

```cs
var otherUsers = users.Take(n);
```
```TypeScript
const otherUsers = users.filter((u, i) => i < n);
```

### TakeWhile

```cs
var otherUsers = users.TakeWhile(predicate);
```
```TypeScript
let i = 0;while (i < users.length && predicate(users[i++]));const otherUsers = users.slice(0, i - 1);
```

### Union

```cs
var allUsers = someUser.Union(otherUsers);
```
```TypeScript
const allUsers = otherUsers.reduce((au, u) =>   au.includes(u) // Use a polyfill for IE support    ? au    : [ ...au, u ]}), someUsers));
```

### Where

```cs
var adults = users.Where(u => u.Age >= 18);
```
```TypeScript
const adults = users.filter(u => u.age >= 18);
```

### Zip

```cs
var matches = buyers.Zip(sellers, (b, s) => new { Buyer = b, Seller = s });
```
```TypeScript
const matches = [];for (let i = 0; i < buyers.length && i < sellers.length; i++) {  matches.push({    buyer: buyers[i],    seller: sellers[i],  });}
```