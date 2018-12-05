SecretSanta
===========

A simple console application that resolves a Secret Santa list and distributes each match to the designated gifter via email.

Features
--------
* Supports relationships. People who are in a relationship cannot be each others Secret Santa.
* Supports third-party verification (auditor). Allows a summary of the result to be emailed to someone external to the Secret Santa group. Can be used as a fail-safe in the case of email loss or delivery failure.
 
How to use it
-------------
Most configuration is statically defined in Program.cs. This configuration includes auditor, people and relationships. Simply modify these lists to your liking. SMTP settings can be configured via the applications configuration file (app.config).

Once configured simply run the program and it will perform the matching and then email each gifter. It will finally email the summary to the designated auditor.
