# **Coding part**

## Requirements

NET6-win10-x64 (can be built for any platform but I specified win10-x64 to have an .exe output file)

## Compiling Source

~~~bash
$ dotnet build 
~~~

## Running unit tests

~~~bash
$ cd UnifiedProductCatalog.ImportTool.UnitTests 
$ dotnet test
~~~

## Running End2End tests

~~~bash
$ cd UnifiedProductCatalog.ImportTool.End2EndTests 
$ dotnet test
~~~

## Publish to be used as executable for win10-x64

~~~bash
$ cd UnifiedProductCatalog.ImportTool 
$ dotnet publish -c Release -r win10-x64 -o <desired output directory>
~~~

## Explanations, considerations and possible future improvements

First of all I'd like to focus on Models.
I assumed that the idea was to unify data models from multiple sources to a unified common model that could represent data coming form anywhere. 

That's why I wrote *CapterraDeserializer.cs* and *SoftwareAdviceProduct.cs* classes used to read information from Capterra and SoftwareAdvice sources respectively and *Product.cs* is meant to be this "unified" product model to store data inside the Unifided Product Catalog.

The excercise asked me to keep in mind that other data providers could be added in the future. That's why I implemented a factory pattern. *ProvidersDeserializersFactory.cs* depending on the provider name, returns back an *IProviderDeserializer* which have the logic to read the corresponding file for each provider, map the custom model to the unified one and return it.

So when a new data provider starts working with the Unifided Promotion Catalog, we just have to implement a new *IProviderDeserializer* and add it to the internal dictionary. Eventually if DI would be added to the command line application, this internal dictionary should be substituted by some DI container configuration depending on the library used.

The excercise also asked me to keep in mind that persistence model could change in the future switching from MySql to MongoDB. That's why I separated the persistance layer from the deserializers and I implemented a repository pattern. 

The *IProductRepository* interface is an abstraction for all data persistence methods that we could need in the future. In this case the only implementation for this interface is *EchoProductRepository.cs* which just echoes to the console the desired output text asked by the exercise.

So when MongoDb comes we should only provide an implementation of *IProductRepository* that uses MongoDb internally.

### **Possible improvements**

I did not apply any complex internal architecture cause  I did not find it necesary. I think SOLID principles are applied all over my code and it's simple enough for anybody to understand what each class does, but if the project grows it may require some refactor and I would maybe think about implementing some internal structure with mutiple projects following vertical slice for each command or provider or maybe implmenting some "Onion/Ports And Adapters/Hexagonal/etc..." Horizontal architecture to have higher consistency across all internal design of the application.

For these kind of command line applications I would try to use libraries to ease the task of parsing parameters like [(natemcmaster - Command Line Utils)](https://github.com/natemcmaster/CommandLineUtils) which helps these command line tools look much more like a professional command line tool, adding "help" commands and providing functionality for parsing multiple kind of parameters, etc...

Also when asynchronous interfaces would appear like asking information to a REST service I would change synchronous interfaces of *IProviderDeserializer* for asynchronous ones.

And last but not least I might try to find a different Yaml parsing library the one I choose seems to be a litte bit old and does not support directly latest .NET version. There are some Pull requests on the way https://github.com/aaubry/YamlDotNet/pull/630 but it's opened since 2021 so that could mean the project it's not maintained anymore.

Do not hesitate to contact me for any explanation.

# **SQL part - Find answers in quoted sections below each exercise**

1. Select users whose id is either 3,2 or 4
- Please return at least: all user fields

~~~sql
SELECT * FROM `users` WHERE `id` in (3,2,4);
~~~

2. Count how many basic and premium listings each active user has
- Please return at least: first_name, last_name, basic, premium
~~~sql
SELECT first_name, last_name, (SELECT COUNT(0) FROM listings WHERE user_id = u.id AND status = 2) as basic, (SELECT COUNT(0) FROM listings WHERE user_id = u.id AND status = 3) as permium FROM `users` as u;
~~~
3. Show the same count as before but only if they have at least ONE premium listing
- Please return at least: first_name, last_name, basic, premium
~~~sql
SELECT * FROM (SELECT first_name, last_name, 
              (SELECT COUNT(0) FROM listings WHERE user_id = u.id AND status = 2) as basic, 
              (SELECT COUNT(0) FROM listings WHERE user_id = u.id AND status = 3) as premium 
               FROM `users` as u) as unfiltered_temporary_table
               WHERE result1.premium > 0;
~~~
4. How much revenue has each active vendor made in 2013
- Please return at least: first_name, last_name, currency, revenue
~~~sql
SELECT first_name, last_name, currency, SUM(c.price) FROM users u INNER JOIN listings l ON u.id = l.user_id INNER JOIN clicks c ON l.id = c.listing_id GROUP BY u.first_name, u.last_name, c.currency;
~~~
5. Insert a new click for listing id 3, at $4.00
- Find out the id of this new click. Please return at least: id
~~~sql
INSERT INTO clicks (created, currency, listing_id, price) VALUES(CURRENT_DATE(), 'USD', 3, 4.00);
~~~
6. Show listings that have not received a click in 2013
- Please return at least: listing_name
~~~sql
SELECT * FROM listings l WHERE l.id not in (SELECT l.id FROM listings l inner join clicks c on l.id = c.listing_id WHERE YEAR(c.created) = 2013);
~~~
7. For each year show number of listings clicked and number of vendors who owned these listings
- Please return at least: date, total_listings_clicked, total_vendors_affected
~~~sql
SELECT YEAR(c.created) year, count(c.id) total_listings_clicked, count(distinct l.id) total_vendros_affected FROM users u inner join listings l on u.id = l.user_id inner join clicks c on l.id = c.listing_id GROUP BY year;
~~~
8. Return a comma separated string of listing names for all active vendors
- Please return at least: first_name, last_name, listing_names
~~~sql
SELECT first_name, last_name, GROUP_CONCAT(l.name) from users u inner join listings l on u.id = l.user_id GROUP BY first_name, last_name;
~~~