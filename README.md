# DrugsInteractionApi
## To test api you should send GET requests

like

/api/foodInteractions?DrugName=Віагра, анальгін

/api/foodInteractions?DrugName=Віагра

/api/search?drugName=мез

(don't ask why route is "foodInteractions", it was customer requirement)

You can see examples how to use it in this postman folder: [click](https://api.postman.com/collections/22202583-b2b9758e-a6e3-4c09-b191-9b150f3b7b2f?access_key=PMAT-01H5GR5PNB7B7PQRJ57Y0SKRGF)

(you need to import folder in postman)


Sensitive data like auth key is in environment,
As well as host, but it for my testing purposes, currently api is hosted on https://www.apiipaapiipa.pp.ua
## API key
Api require auth key, but i won't tell you. you can use dummy key
Just set header 

````
Authorization="dummy-key"
````

There is limit: no more then 100 requests with this key per day

# Have Fun :)
