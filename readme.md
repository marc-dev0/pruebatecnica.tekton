## Source

https://github.com/marc-dev0/pruebatecnica.tekton

## Pasos para la instalación de los requisitos para ejecutar el programa y las pruebas unitarias
1.- Clonar el repositorio:\
git clone https://github.com/marc-dev0/pruebatecnica.tekton.git

2.- Entrar a la carpeta clonada\
cd .\pruebatecnica.tekton\

4.- Restaurar los paquetes nuget\
dotnet restore

5.- Entrar a la carpeta del proyecto WebApi\
cd src\Tekton.Api

6.- Ejecutar la bd local EF
dotnet ef migrations add ProductTable

7.- Actualizar bd local EF
dotnet ef database update

## Consideraciones
1.- En el repositorio raiz del git se encuentra la colección json (Tekton.postman_collection) para cargar los endpoints y testear