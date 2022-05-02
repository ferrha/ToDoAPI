# Questions

* How long did you spend on your solution?
	I have spent over 6 hours, but more than 2 hours have been used due to problems with tests.

* How do you build and run your solution?
	To build and run the solution it is necessary to choose Debug/Any CPU/PerfectChannel.WebApi and press F5.

* What technical and functional assumptions did you make when implementing your solution?
	I have used an in memory database to store data. 
	In the user story 1, when there are not tasks added, the empty object returned contains empty completed and pending objects.
	And the task model includes id, name, description and status, allways returned, that in my opinion gives the more information the API offer.

* Explain briefly your technical design and why do you think is the best approach to this problem.
	In my desing I have implemented a model with ID key, Name, Description and Status. Status is set to false by default. I think this model is simple and complete, giving all the information required about a task.
	To store data I have used an in memory database because I think it is easy and faster to implement if it is not necessary data to persist.
	In the controller, I have implemented 3 endpoints, one per user story: 
		User Story 1 - Method GetToDoItems(): Get all tasks from the database and group by ItemStatusCompleted, true ones are shown as completed and false ones are shown as pending. Grouping has been implementd using Ling and lambda operations, which I think is not optimal but I did not know any other way.
		User Story 2 - Method PostToDoItem(): Insert a new task, save changes and return 201 code and the added object.
		User Story 3 - Method PutToDoItem(): Update the status of a task by ItemId, change the status to the opossite and return the updated object. If a task is not found by ItemId passed as parameter, method returns not found (404) code.
	Finally, controller endpoints are implemented without routes, simplifying the name of the endpoints and the use of the API.

* If you were unable to complete any user stories, outline why and how would you have liked to implement them.
	I think, every user story is completed as asked and every endpoint has a test. I would like to write more test, but I do not know how to write another tests.
	And about the GetToDoItems endpoint, I think it should be optimized but I can not think in another way to do it.
