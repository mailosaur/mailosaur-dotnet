mailosaur-dotnet
================

Mailosaur .NET bindings

See mailosaur.net for more info.

Api:

Email[] GetEmails(String searchPattern)
Retrieves all emails which have the searchPattern text in their body or subject.

Email[] GetEmailsByRecipient(String recipientEmail)
Retrieves all emails sent to the given recipient.

Email GetEmail(String emailId)
Retrieves the email with the given id.

Void DeleteAllEmail()
Deletes all emails in a mailbox.

Void DeleteEmail(String emailId)
Deletes the email with the given id.

Byte[] GetAttachment(String attachmentId)
Retrieves the attachment with specified id.

Byte[] GetRawEmail(String rawId)
Retrieves the complete raw EML file for the rawId given. RawId is a property on the email object.

String GenerateEmailAddress()
Generates a random email address which can be used to send emails into the mailbox.
