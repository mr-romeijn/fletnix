DELETE FROM Movie
DELETE FROM Person
DELETE FROM Movie_Cast
DELETE FROM Movie_Director
DELETE FROM MovieAward

select top 100000 * from Movie order by publication_year desc

select top 100000 * from Person where lastname IS NOT NULL

select top 100000 * from Movie_Cast WHERE Movie_Cast.person_id IN (select top 100000 Person.person_id from Person where lastname IS NOT NULL) AND Movie_Cast.movie_id IN (select top 100000 movie_id from Movie order by publication_year desc)

select top 100000 * from MovieAward WHERE MovieAward.person_id IN (select top 100000 Person.person_id from Person where lastname IS NOT NULL) AND MovieAward.movie_id IN (select top 100000 movie_id from Movie order by publication_year desc)

select top 100000 * from Movie_Genre WHERE Movie_Genre.movie_id IN (select top 100000 movie_id from Movie order by publication_year desc)

select top 100000 * from Movie_Director WHERE Movie_Director.person_id IN (select top 100000 Person.person_id from Person where lastname IS NOT NULL) AND Movie_Director.movie_id IN (select top 100000 movie_id from Movie order by publication_year desc)





//////OUD
select top 100000 * from Movie_Cast WHERE Movie_Cast.movie_id IN (select top 100000 Movie.movie_id from Movie order by publication_year desc) order by Movie_cast.movie_id desc

select top 100000 * from MovieAward WHERE MovieAward.movie_id IN (select top 100000 Movie.movie_id from Movie order by publication_year desc) order by MovieAward.movie_id desc

select top 100000 * from Movie_Genre WHERE Movie_Genre.movie_id IN (select top 100000 Movie.movie_id from Movie order by publication_year desc) order by Movie_Genre.movie_id desc

select top 100000 * from Movie_Director WHERE Movie_Director.movie_id IN (select top 100000 Movie.movie_id from Movie order by publication_year desc) order by Movie_Director.movie_id desc

select top 100000 * from Person WHERE lastname IS NOT NULL AND Person.person_id IN (select top 100000 Movie_Cast.person_id from Movie_Cast WHERE Movie_Cast.movie_id IN (select top 100000 Movie.movie_id from Movie order by publication_year desc) order by Movie_cast.movie_id desc) ORDER BY person_id desc 

SELECT TOP 100000 * FROM Movie_Cast WHERE Movie_Cast.person_id IN (select top 100000 Person.person_id from Person WHERE lastname IS NOT NULL ORDER BY person_id desc) AND Movie_Cast.movie_id IN (select top 100000 Movie.movie_id from Movie order by publication_year desc)


SELECT TOP 100000 * FROM Movie_Director WHERE Movie_Director.person_id IN (select top 100000 Person.person_id from Person WHERE lastname IS NOT NULL ORDER BY person_id desc) AND Movie_Director.movie_id IN (select top 100000 Movie.movie_id from Movie order by publication_year desc)

SELECT TOP 100000 * FROM MovieAward WHERE MovieAward.person_id IN (select top 100000 Person.person_id from Person WHERE lastname IS NOT NULL ORDER BY person_id desc) AND MovieAward.movie_id IN (select top 100000 Movie.movie_id from Movie order by publication_year desc)
///////