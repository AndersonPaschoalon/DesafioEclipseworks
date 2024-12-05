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


##  Rodando a aplica��o em um Container Docker (Windows)

Para utilizar o container Docker estamos trabalhando no SO Windows, com os seguintes requisitos:
- Docker Desktop para Windows;
- WSL 2 com Ubunto 24.04-LTS


Depois que todas as depend�ncias foram instaladas corretamente, devemos criar a imagem Docker.  
Partindo do diret�rio contendo o arquivo de solu��o (EclipseTaskManager.sln), vamos para o diret�rio do  projeto:

```
cd EclipseTaskManager
```

Em seguida, criamos a imagem Docker com o seguinte comando:

```
docker build -t eclipsetaskmanager.image .
```

Por fim, criamos o container Docker com o seguinte comando:

```
docker run -it --rm -p 18080:8080 --name eclipsetaskmanager.docker eclipsetaskmanager.image
```

Escolhemos a porta `18080` para acessar o container na maquina local, e a porta 8080 para serutilizada pelo container.

Uma vez rodando o container, podemos testar o acesso ao EclipseTaskManager realizando a listagem de usu�rios pela url:
```
http://localhost:18080/v1/api/Users
```

TODO: connection
**IMPORTANTE**: Ser� necess�rio se atualizar o arquivo "appsettings.json" para que a aplica��o se conecte a base de dados, caso ela n�o esteja localizada no localhost.


## Refinamento com o PO

1. Atualmente um projeto n�o pode ter mais que 20 tasks, mesmo que elas estejam em Done. Voc� acredita que seria interessante modificar essa regra de neg�cio, contando somente tarefas em "ToDo" e "InProgress" para se realizar essa limita��o de 20 tarefas?
1. Caso o n�mero maximo de tarefas realmente corresponda a soma das tarefas "InProgress" e "ToDo", creio que ser� necess�rio adicionar uma nova valida��o que impe�a uma tarefa de ter seu estado modificado de "Done" para "Todo" ou "InProgress" caso j� existam 20 tarefas nestes estados.
1. Haveriam mais informa��es relevantes a serem rastreadas pelo relat�rio? 
1. Seria interessante gerar um relat�rio no fomato PDF?
1. Cria��o de um mecanismo de autentica��o? Da forma que a implementa��o foi realizada, o sistema est� vulner�vel a um ataque malicioso.
1. Melhorias de arquitetura e d�bito t�cnico: verificar se��o ""Final: Identifica��o de melhorias"".


## Final: Identifica��o de melhorias

1. Implementa��o de uma bibliot�ca de logs padr�o de mercado, como o Log4Net
1. Aumento da cobertuda dos testes de unidade
1. Implementa��o de mecanismos OWASP para tornar o sistema mais resilientes a ataques.




