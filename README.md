## How To Run

1. Run `docker-compose up` in the IndexRepro project directory
2. Start the IndexRepro Web API project
3. Wait for initial data to be seeded
4. Make a GET request to the repro endpoints with a couple of diffferent marten queries

# Domain
The repro project contains two simple documents `Game` and `Genre`. The `Game` document has a `Guid[]` which references one or more Genre identifiers.
In my application, I am trying to query for **Games** given a genre Id. For example: Give me all games which are in the "Racing" genre.

When calling the endpoints, postgres performs an index scan and takes 5+ seconds to give me back results on a moderately sized table which has a `gin` index I would expect to be used.


## Endpoints

* http://localhost:5200/games/contains-query — Attempts to query using `.Contains()`
* http://localhost:5200/games/any-query — Attempts to query using `.Any()`
* http://localhost:5200/games/is-one-of-query — Attempts to query using `.IsOneOf()` **_(At the moment I get a runtime exception here. I could be doing something wrong but left the code in place)_**
