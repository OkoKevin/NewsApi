# News API

NewsAPI returns news articles from the GNews API.
It includes 3 Endpoints:

### News/articles
Requires a 'limit' query parameter between 1 and 100. Returns a number of news articles depending on the limit
### News/articles/search
Searches for news articles filtered by the keyword in the 'keyword' parameter. Returns news articles filtered by the keyword in the 'keyword' parameter.
### News/articles/bydate
Filters news articles by date. Requires a 'from' and a 'to' date in the following format: 2022-08-21T16:27:09Z Returns news articles filtered by 'from' and 'to' date.

## Installation
1. Pull this repository
2. Open CLI
3. Navigate to the project folder that includes the docker-compose.yml file
4. Run the API as a docker container by running "docker-compose up -d" from the CLI
5. Use the API with the http://localhost:8080/Swagger user interface 
