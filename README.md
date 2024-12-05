# Desafio Eclipseworks

## Sumário

1. Introdução
1. Overview da aplicação e dos endpoints
1. Overview da Base de Dados
1. Caso de uso: Ciclo de vida do gerenciamento de um projeto, e criação de relatórios
1. Rodando a aplicação em um Container Docker
1. Refinamento com o PO
1. Final: Identificação de melhorias

## Introdução

A aplicação EclipseTaskManager tem por objetivo fornecer aos seus usuários um serviço de criação de projetos, e tarefas associadas a esses projetos, de forma que seus clientes possam controlar o ciclo de vida das atividas cadastradas.
Além disso o serviço permite:

- Que o usuário seja capaz adicionar comentários vinculados a uma determinada tarefa;

- Todas as alterações realizadas nos atributos de uma Task são registradas no histórioco de atualizações dessa mesma task. Cada entrada desse histórico de atualizações armazena as seguintes informações:
	- Nome Campo modificado;
	- Conteúdo original deste campo;
	- Novo valor associado a esse campo;
	- Data de modificação;
	- Usuário, projeto e task que essas modificações estão associadas.
	Nesse sentido, alterações ou inserções de comentários também são condideradas modificações, e portanto são controladas pelo sistema de gerenciamento do histórico de alterações.


##  Overview da aplicação e dos endpoints

A arquitetura escolhida para aplicação seguiu o padrão MVC, no qual foram criados os seguintes controladores:
- *UsersController*: responsável pelas operações e gerenciamento dos usuários;
- *ProjectsController*: Responsável pelas operações e gerenciamento dos projetos;
- *ProjectTasksController*: Responsável pelas operações e gerenciamento dos das tarefas associadas aos projetos;
- *ProjectTaskCommentsController*: Responsável pelas operações e gerenciamento dos comentários adicionadas a uma determinada tarefa;
- *ReportsController*: Responsável por gerar os relatórios ao usuários do tipo Admin

- Os endipoints são associados ao gerenciamento do ciclo de vida dos projeto. Vamos passar brevemente por caso um deles.



#### Users

```
GET v1/api/Users
```
> Reliza a listagem de todos os usuários cadastrados no sistema. 

```
GET v1/api/Users/{id}
```
> Recupera as informações de um usário com ID especificado.

```
POST v1/api/Users
```
> Realiza o cadastro de novos usuários. O campo Role pode ser "0" para usuários do tipo admin e "1" para usuários do tipo "Consumer".

```
PUT v1/api/Users
```
> Realiza atualização de cadastro de usuários. O ID do usuário deve ser passado no corpo da requisição.


#### Projects

```
GET v1/api/Projects/userId?userId={userId}
```
> Realiza a listagem de TODOS os Projetos associados com um Usuário especificado por seu ID, espefificado atrabés do query parameter da mensagem.

```
GET v1/api/Projects/{userId}
```
> Realiza a listagem de UM projeto especificado por seu proprio ID.

```
POST v1/api/Projects
```
> Realiza o cadastro de um projeto. Nele, o cliente deverá especificar o seu titulo, uma descrição e o seu ID de usuário.

```
PUT v1/api/Projects
```
> Atualiza o cadastro de um projeto. O cliente deverá fornecer os novos valores dos campos, e passar o ID do projeto pelo corpo da mensagem.

```
DELETE v1/api/Projects
```
> Apaga um projeto cadastrado. Para o projeto ser apagado é necessário que não exista mais Tarefa associadas a esse projeto, caso contrario, a deleção não será concluida. 


#### Project Tasks

```
GET v1/api/ProjectTasks/projectId?projectId={projectId}
```
> Realiza a listagem de todas as Tarefas associadas com om projeto especificado pelo seu ID. Este método GET retorna apenas a listagem das tarefas, MAS NÃO LISTA SEUS COMENTÁRIOS NEM SEU HISTÓRICO DE MUDANÇAS, para economizar banda.

```
GET v1/api/ProjectTasks/{id}
```
> Realiza a listagem de uma Tarefa recuperada através de seu ID.
> Através desse método é possível **VIZUALIZAR OS COMENTÁRIOS E HISTÓRICOS DE ALTERAÇÕES ASSOCIADOS COM A TAREFA**. Estes campos são listados logo abaixo dos dados principais das tarefas, nos campos "comment" e "updates". 

```
POST v1/api/ProjectTasks
```
> Cria uma nova Tarefa, que deve ser associada a um projeto e usuário. Deve possuir um titulo, uma descrição, e uma data para conclusão. 
> Deve possuir um Status (0:Todo[default], 1:InProgress, 2:Done).
> Deve possuir uma Prioridade (0:Low[default], 2:Medium, 3:High).
> Campos `creationDate`, `comment` e `updates` são ignorados.
> Cada projeto pode ter no máximo 20 Tarefas. Cado ele tenha mais que 20 tarefas, a nova tarefa não será inserida. Para isso, alguma das tarefas terão que ser deletadas.
> Dados inseridos via POST não são cadastrados no histórico de mudanças.

```
PUT v1/api/ProjectTasks
```
> Responsável por atualizar uma tarefa. O ID da mensagem a ser autalizada é passado no corpo da mensagem `projectTaskId`.
> O serviço irá identificar os campos que foram alterados, e irá criar entradas na table de histórico de mudanças. Este histórico de mudanças pode ser consultado atráves do endpoint `GET v1/api/ProjectTasks/{id}` já mencionado acima. Importante, uma operação de PUT pode gerar diversas entradas no historico de mudanças, uma vez que elas são associadas aos campos alterados.
> Não é possivel alterar o Projeto que uma Tarefa está associada. 

```
DELETE v1/api/ProjectTasks/{id}
```
> Deleta uma rarefa de um projeto.


#### ProjectTaskComments

Os comentários associados às tarefas devem ser feitos por meio desse controlador. 

```
GET v1/ProjectTaskComments
``` 
> Faz a listagem dos comentarios cadastradas na base de dados.

```
POST v1/ProjectTaskComments
``` 
> Realiza o cadastro de um novo comentário. Comentários adicionados via POST **SÃO CONSIDERADOS MODIFICAÇÕES DE UMA TAREFA, E PORTANTO SÃO ADICIONADOS AO HISTÓRICO DE MUDANÇAS**.

```
PUT v1/ProjectTaskComments
``` 
> Atualiza um comentário. Somente o comentário pode ser atualizado através desta operação. Qualquer atualização será registrada no histórico de mudanças.

```
GET v1/ProjectTaskComments/{id}
``` 
> Lista um comentário especificado por seu ID.

```
DELETE v1/ProjectTaskComments/{id}
``` 
> Deleta um comentário, especificado por seu ID.


#### Reports

```
GET v1/api/Reports/userId?userId={id}&daysPrior={days}
```
> Retorna um relatório de desempenho. É possivel se especificar o intervalo de  tempo (em dias) que será utilizado para se gerar esse relatório, sendo o numero de dias anteriores a data atual a serem considerados. 
> O usuário especificado nos query parameters correspode ao **USUÁRIO QUE ESTÁ REALIZANDO A CONSULTA**. Caso o usuário especificado seja um usuário do tipo Consumer, o acesso ao relatório seá barrado. Se ele for Admin, poderá ter acesso aos dados.
> Atualmente o relatório fornece uma listagem de todas as Tasks completadas (no estado "Done"), bem como o número médio de Tasks postas em "Done" pelos usuários.


##  Overview da Base de Dados

Foram criadas no total 5 tabelas para armazenamento das informações, bem como uma tabela adicional criada pelo entity framework para armazenamento das informações  das Migrations. 
São as tabelas:
- Projects
- ProjectTaskComments
- ProjectTasks
- ProjectTasksUpdates
- Users
E a tabela gerada pelo entity framework:
- __efmigrationshistory
Os endipoints manipulas diretamente as tabelas "Projects", "ProjectTaskComments", "ProjectTasks" e "Users". A tabela "ProjectTasksUpdates" é manipulada indiretamente pelos endpoints associados as tabelas "ProjectTaskComments" e "ProjectTasks", e não possui endpoints proprios. 
A tabela ProjectTasks também possui uma coluna "ConclusionDate" que armazena quando a Task foi posta no estado "Done". Essa informação é importante para o relatório a ser gerado.


##  Caso de uso: Ciclo de vida do gerenciamento de um projeto, e criação de relatórios


##  Rodando a aplicação em um Container Docker (Windows)

Para utilizar o container Docker estamos trabalhando no SO Windows, com os seguintes requisitos:
- Docker Desktop para Windows;
- WSL 2 com Ubunto 24.04-LTS


Depois que todas as dependências foram instaladas corretamente, devemos criar a imagem Docker.  
Partindo do diretório contendo o arquivo de solução (EclipseTaskManager.sln), vamos para o diretório do  projeto:

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

Uma vez rodando o container, podemos testar o acesso ao EclipseTaskManager realizando a listagem de usuários pela url:
```
http://localhost:18080/v1/api/Users
```

TODO: connection
**IMPORTANTE**: Será necessário se atualizar o arquivo "appsettings.json" para que a aplicação se conecte a base de dados, caso ela não esteja localizada no localhost.


## Refinamento com o PO

1. Atualmente um projeto não pode ter mais que 20 tasks, mesmo que elas estejam em Done. Você acredita que seria interessante modificar essa regra de negócio, contando somente tarefas em "ToDo" e "InProgress" para se realizar essa limitação de 20 tarefas?
1. Caso o número maximo de tarefas realmente corresponda a soma das tarefas "InProgress" e "ToDo", creio que será necessário adicionar uma nova validação que impeça uma tarefa de ter seu estado modificado de "Done" para "Todo" ou "InProgress" caso já existam 20 tarefas nestes estados.
1. Haveriam mais informações relevantes a serem rastreadas pelo relatório? 
1. Seria interessante gerar um relatório no fomato PDF?
1. Criação de um mecanismo de autenticação? Da forma que a implementação foi realizada, o sistema está vulnerável a um ataque malicioso.
1. Melhorias de arquitetura e débito técnico: verificar seção ""Final: Identificação de melhorias"".


## Final: Identificação de melhorias

1. Implementação de uma bibliotéca de logs padrão de mercado, como o Log4Net
1. Aumento da cobertuda dos testes de unidade
1. Implementação de mecanismos OWASP para tornar o sistema mais resilientes a ataques.




