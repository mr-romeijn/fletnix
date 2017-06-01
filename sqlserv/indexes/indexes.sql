CREATE NONCLUSTERED INDEX IX_MOVIE_PUBYEAR ON Movie (publication_year)
CREATE NONCLUSTERED INDEX IX_MOVIE_ID ON Movie (movie_id)
CREATE NONCLUSTERED INDEX IX_MOVIE_ID ON MovieAward (movie_id)
CREATE NONCLUSTERED INDEX IX_CUSTOMER_EMAIL ON Customer (customer_mail_address)
CREATE NONCLUSTERED INDEX IX_PERSON_ID ON Person (person_id)
CREATE NONCLUSTERED INDEX IX_CUSTOMER_MOVIE ON Movie_Review (movie_id,customer_mail_address)
CREATE NONCLUSTERED INDEX IX_WATCHHISTORY_WATCHDATE ON Watchhistory (watch_date)

CREATE FULLTEXT CATALOG ftCatalog AS DEFAULT; 
CREATE FULLTEXT INDEX ON Movie  
 (   
  title Language 1033,  
  description Language 1033     
 )  
  KEY INDEX PK_Movie_1
      ON ftCatalog

SELECT Movie.movie_id, publication_year, title, MovieAward.name, MovieAward.type, MovieAward.result FROM Movie LEFT JOIN MovieAward ON Movie.movie_id = MovieAward.movie_id WHERE publication_year between 1901 AND 2018
