# DishChallenge

Just a challenge!

An instance is hosted at this address:

<http://dishchallenge.herokuapp.com/total>

## Notes

## Build docker image

~~~bash
docker build -t dishchallenge .
~~~

## Run image

~~~bash
docker run -it --rm -p 5000:80 --name dishchallengecontainer dishchallenge
~~~

## Manual deploy to Heroku

~~~bash
heroku login
docker ps
heroku container:login
heroku container:push web -a dishchallenge
heroku container:release web -a dishchallenge
~~~
