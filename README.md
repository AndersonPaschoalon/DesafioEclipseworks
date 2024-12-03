# Desafio Eclipseworks

## Sum�rio

1. Introdu��o
1. Overview da aplica��o e dos endpoints
1. Overview da Base de Dados
1. Caso de uso: Ciclo de vida do gerenciamento de um projeto, e cria��o de relat�rios
1. Rodando a aplica��o em um Container Docker
1. Refinamento com o PO
1. Final: Identifica��o de melhorias

## Introdu��o

A aplica��o EclipseTaskManager tem por objetivo fornecer aos seus usu�rios um servi�o de cria��o de projetos, e tarefas associadas a esses projetos, de forma que seus clientes possam controlar o ciclo de vida das atividas cadastradas.
Al�m disso o servi�o permite:

- Que o usu�rio seja capaz adicionar coment�rios vinculados a uma determinada tarefa;

- Todas as altera��es realizadas nos atributos de uma Task s�o registradas no hist�rioco de atualiza��es dessa mesma task. Cada entrada desse hist�rico de atualiza��es armazena as seguintes informa��es:
	- Nome Campo modificado;
	- Conte�do original deste campo;
	- Novo valor associado a esse campo;
	- Data de modifica��o;
	- Usu�rio, projeto e task que essas modifica��es est�o associadas.
	Nesse sentido, altera��es ou inser��es de coment�rios tamb�m s�o condideradas modifica��es, e portanto s�o controladas pelo sistema de gerenciamento do hist�rico de altera��es.


##  Overview da aplica��o e dos endpoints

A arquitetura escolhida para aplica��o seguiu o padr�o MVC, no qual foram criados os seguintes controladores:
- *UsersController*: respons�vel pelas opera��es e gerenciamento dos usu�rios;
- *ProjectsController*: Respons�vel pelas opera��es e gerenciamento dos projetos;
- *ProjectTasksController*: Respons�vel pelas opera��es e gerenciamento dos das tarefas associadas aos projetos;
- *ProjectTaskCommentsController*: Respons�vel pelas opera��es e gerenciamento dos coment�rios adicionadas a uma determinada tarefa;
- *ReportsController*: Respons�vel por gerar os relat�rios ao usu�rios do tipo Admin

- Os endipoints s�o associados ao gerenciamento do ciclo de vida dos projeto. Vamos passar brevemente por caso um deles.

- 
#### Users

```
GET v1/api/Users
```
> Reliza a listagem de todos os usu�rios cadastrados no sistema. 

```
GET v1/api/Users/{id}
```
> Recupera as informa��es de um us�rio com ID especificado.

```
POST v1/api/Users
```
> Realiza o cadastro de novos usu�rios. O campo Role pode ser "0" para usu�rios do tipo admin e "1" para usu�rios do tipo "Consumer".

```
PUT v1/api/Users
```
> Realiza atualiza��o de cadastro de usu�rios. O ID do usu�rio deve ser passado no corpo da requisi��o.


#### Projects

```
GET v1/api/Projects/userId?userId={userId}
```
> Realiza a listagem de TODOS os Projetos associados com um Usu�rio especificado por seu ID, espefificado atrab�s do query parameter da mensagem.

```
GET v1/api/Projects/{userId}
```
> Realiza a listagem de UM projeto especificado por seu proprio ID.

```
POST v1/api/Projects
```
> Realiza o cadastro de um projeto. Nele, o cliente dever� especificar o seu titulo, uma descri��o e o seu ID de usu�rio.

```
PUT v1/api/Projects
```
> Atualiza o cadastro de um projeto. O cliente dever� fornecer os novos valores dos campos, e passar o ID do projeto pelo corpo da mensagem.

```
DELETE v1/api/Projects
```
> Apaga um projeto cadastrado. Para o projeto ser apagado � necess�rio que n�o exista mais Tarefa associadas a esse projeto, caso contrario, a dele��o n�o ser� concluida. 


#### Project Tasks

```
GET v1/api/ProjectTasks/projectId?projectId={projectId}
```
> Realiza a listagem de todas as Tarefas associadas com om projeto especificado pelo seu ID. Este m�todo GET retorna apenas a listagem das tarefas, MAS N�O LISTA SEUS COMENT�RIOS NEM SEU HIST�RICO DE MUDAN�AS, para economizar banda.

```
GET v1/api/ProjectTasks/{id}
```
> Realiza a listagem de uma Tarefa recuperada atrav�s de seu ID.
> Atrav�s desse m�todo � poss�vel **VIZUALIZAR OS COMENT�RIOS E HIST�RICOS DE ALTERA��ES ASSOCIADOS COM A TAREFA**. Estes campos s�o listados logo abaixo dos dados principais das tarefas, nos campos "comment" e "updates". 

```
POST v1/api/ProjectTasks
```
> Cria uma nova Tarefa, que deve ser associada a um projeto e usu�rio. Deve possuir um titulo, uma descri��o, e uma data para conclus�o. 
> Deve possuir um Status (0:Todo[default], 1:InProgress, 2:Done).
> Deve possuir uma Prioridade (0:Low[default], 2:Medium, 3:High).
> Campos `creationDate`, `comment` e `updates` s�o ignorados.
> Cada projeto pode ter no m�ximo 20 Tarefas. Cado ele tenha mais que 20 tarefas, a nova tarefa n�o ser� inserida. Para isso, alguma das tarefas ter�o que ser deletadas.
> Dados inseridos via POST n�o s�o cadastrados no hist�rico de mudan�as.

```
PUT v1/api/ProjectTasks
```
> Respons�vel por atualizar uma tarefa. O ID da mensagem a ser autalizada � passado no corpo da mensagem `projectTaskId`.
> O servi�o ir� identificar os campos que foram alterados, e ir� criar entradas na table de hist�rico de mudan�as. Este hist�rico de mudan�as pode ser consultado atr�ves do endpoint `GET v1/api/ProjectTasks/{id}` j� mencionado acima. Importante, uma opera��o de PUT pode gerar diversas entradas no historico de mudan�as, uma vez que elas s�o associadas aos campos alterados.
> N�o � possivel alterar o Projeto que uma Tarefa est� associada. 

```
DELETE v1/api/ProjectTasks/{id}
```
> Deleta uma rarefa de um projeto.


#### ProjectTaskComments

Os coment�rios associados �s tarefas devem ser feitos por meio desse controlador. 

```
GET v1/ProjectTaskComments
``` 
> Faz a listagem dos comentarios cadastradas na base de dados.

```
POST v1/ProjectTaskComments
``` 
> Realiza o cadastro de um novo coment�rio. Coment�rios adicionados via POST **S�O CONSIDERADOS MODIFICA��ES DE UMA TAREFA, E PORTANTO S�O ADICIONADOS AO HIST�RICO DE MUDAN�AS**.

```
PUT v1/ProjectTaskComments
``` 
> Atualiza um coment�rio. Somente o coment�rio pode ser atualizado atrav�s desta opera��o. Qualquer atualiza��o ser� registrada no hist�rico de mudan�as.

```
GET v1/ProjectTaskComments/{id}
``` 
> Lista um coment�rio especificado por seu ID.

```
DELETE v1/ProjectTaskComments/{id}
``` 
> Deleta um coment�rio, especificado por seu ID.


#### Reports

```
GET v1/api/Reports/userId?userId={id}&daysPrior={days}
```
> Retorna um relat�rio de desempenho. � possivel se especificar o intervalo de  tempo (em dias) que ser� utilizado para se gerar esse relat�rio, sendo o numero de dias anteriores a data atual a serem considerados. 
> O usu�rio especificado nos query parameters correspode ao **USU�RIO QUE EST� REALIZANDO A CONSULTA**. Caso o usu�rio especificado seja um usu�rio do tipo Consumer, o acesso ao relat�rio se� barrado. Se ele for Admin, poder� ter acesso aos dados.
> Atualmente o relat�rio fornece uma listagem de todas as Tasks completadas (no estado "Done"), bem como o n�mero m�dio de Tasks postas em "Done" pelos usu�rios.


##  Overview da Base de Dados

Foram criadas no total 5 tabelas para armazenamento das informa��es, bem como uma tabela adicional criada pelo entity framework para armazenamento das informa��es  das Migrations. 
S�o as tabelas:
- Projects
- ProjectTaskComments
- ProjectTasks
- ProjectTasksUpdates
- Users
E a tabela gerada pelo entity framework:
- __efmigrationshistory
Os endipoints manipulas diretamente as tabelas "Projects", "ProjectTaskComments", "ProjectTasks" e "Users". A tabela "ProjectTasksUpdates" � manipulada indiretamente pelos endpoints associados as tabelas "ProjectTaskComments" e "ProjectTasks", e n�o possui endpoints proprios. 
A tabela ProjectTasks tamb�m possui uma coluna "ConclusionDate" que armazena quando a Task foi posta no estado "Done". Essa informa��o � importante para o relat�rio a ser gerado.


##  Caso de uso: Ciclo de vida do gerenciamento de um projeto, e cria��o de relat�rios

TODO

##  Rodando a aplica��o em um Container Docker

Aqui assumimos que as depend�ncias do .NET Core e do ASP.NET j� estejam instaladas em uma VM Linux. 
Caso a configura��o da VM Linux seja necess�ria, recomendo esse artigo: https://hbayraktar.medium.com/how-to-dockerize-a-net-8-asp-net-core-web-application-b15f63246535

Para se instalar o servi�o, primeiramente copiei toda a pasta da solu��o apra a VM LINUX. Estando na pasta do arquivo de solu��o "EclipseTaskManager.sln", realizo um cd para a pasta de projeto:
```
cd EclipseTaskManager
```
verifico que o arquivo de projeto "EclipseTaskManager.csproj" e o Dockerfile est�o presentes nesta pasta, atrav�s de um `ls`.

Em seguida execuot o seguinte comando para criar a imagem Docker:

```
docker build -t eclipse-task-manager .
```

Por fim, executo a imagem Docker utilizando o comando:
```
docker run -d -p 8081:80 --name dotnet8_eclipse_task_manager eclipse-task-manager
```
O servi�o estar� presente na porta 8081.

**IMPORTANTE**: Ser� necess�rio se atualizar o arquivo "appsettings.json" para que a aplica��o se conecte a base de dados, caso ela n�o esteja localizada no localhost.


## Refinamento com o PO

1. Atualmente um projeto n�o pode ter mais que 20 tasks, mesmo que elas estejam em Done. Voc� acredita que seria interessante modificar essa regra de neg�cio, contando somente tarefas em "ToDo" e "InProgress" para se realizar essa limita��o de 20 tarefas?
1. Caso o n�mero maximo de tarefas realmente corresponda a soma das tarefas "InProgress" e "ToDo", creio que ser� necess�rio adicionar uma nova valida��o que impe�a uma tarefa de ter seu estado modificado de "Done" para "Todo" ou "InProgress" caso j� existam 20 tarefas nestes estados.
1. Haveriam mais informa��es relevantes a serem rastreadas pelo relat�rio? 
1. Seria interessante gerar um relat�rio no fomato PDF?
1. Cria��o de um mecanismo de autentica��o? Da forma que a implementa��o foi realizada, o sistema est� vulner�vel a um ataque malicioso.
1. Melhorias de arquitetura e d�bito t�cnico: verificar se��o ""Final: Identifica��o de melhorias"".


## Final: Identifica��o de melhorias

1. Quando uma Task � deletada, os seus Coment�rios e Updates n�o est�o sendo deletados em cascata. � muito importante que essa implementa��o seja realizada antes de ser colocado em produ��o. Ou ent�o, caso n�o seja possivel, e a aplica��o deva ser colocada em produ��o imeditamente, podemos adicionar um script em Migrations que elimina coment�rios e Updates de tasks que j� foram deletadas. 
1. Seria fundamental se separar os objetos de tr�fego de dados (DTOs) dos objetos das entidades. No momento os mesmos objetos est�o sendo utilizados por uma quest�o de agilidade no desenvolvimento, por�m este � um d�bito t�cnico muito significativo, pois dificulta muito a evolu��o da solu��o. Para isso seria necess�rio se implementar essa segregra��o em uma API v2, totalmente desvinculada das entidades. Desta forma seria simples alterar a base de dados ou o modelo de transfer�ncia de dados, sem que um impacte no outro. 
1. Melhorar o mecanismo de inje��o de depend�ncia do contexto, pois da forma que estra primeira vers�o foi desenvolvida a tecnologia de base de dados (MySQL) est� FORTEMENTE acoplada a regra de neg�cio. O ideal � que o acoplamento da tecnologia e da regra de neg�cio seja minimo, de forma que possamos realizar a troca de tecnologia de forma simples. Al�m disso, isso agrega valor ao produto, uma vez que um novo cliente pode querer utilizar outro tipo de banco de dados. 
1. Implementa��o de mecanismos OWASP para tornar o sistema mais resilientes a ataques.




