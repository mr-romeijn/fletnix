CREATE TABLE Movie_Review (
    id int NOT NULL IDENTITY (1,1),
    customer_mail_address varchar(255) NOT NULL,
   	 movie_id int,
    rating int,
    review varchar(500),
	review_date datetime
    PRIMARY KEY (customer_mail_address, movie_id),
   
);

 constraint fk_watchhistory_single_review foreign key (customer_mail_address, movie_id) references dbo.Watchhistory(customer_mail_address,movie_id),

PK__Movie_Re__EEF4C2D2989A55B2