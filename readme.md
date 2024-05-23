Build docker image:

``
docker build -t vidconnect-csharp .
``

Run docker:

``
docker run -d -p 8080:80 --name vidconnectapi-csharp vidconnect-csharp
``