## Source

https://github.com/marc-dev0/pruebatecnica.tekton

## Pasos para la instalación de los requisitos para ejecutar el programa y las pruebas unitarias
1.- Clonar el repositorio:\
git clone https://github.com/marc-dev0/pruebatecnica.tekton.git

2.- Entrar a la carpeta clonada\
cd .\pruebatecnica.tekton\

4.- Restaurar los paquetes nuget\
dotnet restore

5.- Instalar reportgenerator\
dotnet tool install --global dotnet-reportgenerator-globaltool --version 5.2.0

6.- Ejecutar los test\
dotnet test --collect "XPlat Code Coverage"

7.- Buscamos la ultima carpeta generada con el xml generado por la ejecución del test en la ruta\
test/TestResults

8.- Capturar el nombre de la carpeta y reemplazarlo en el siguiente codigo que esta entre [[]]\
reportgenerator -reports:.\test\TestResults/[[reemplazarcodigo]]/coverage.cobertura.xml -targetdir:.\test/CoverageReport -reporttypes:html

9.- Quedaria asi y ejecutarlo\
reportgenerator -reports:.\test\TestResults\352f878e-4e78-4bdb-a473-649f97915a69/coverage.cobertura.xml -targetdir:.\test/CoverageReport -reporttypes:html

10.- En la carpeta CoverageReport buscar el archivo index.html y abrirlo\
test/CoverageReport

https://postimg.cc/2q7bw0K3

## Consideraciones

- El branch coverage ha llegado a 90% porque hay algunas clases que tienen metodos
privados
- El line coverage llego a 97%