using RestSharpServices;
using System.Net;
using System.Reflection.Emit;
using System.Text.Json;
using RestSharp;
using RestSharp.Authenticators;
using NUnit.Framework.Internal;
using RestSharpServices.Models;
using System;
using System.ComponentModel.Design;

namespace TestGitHubApi
{
    public class TestGitHubApi
    {
        private GitHubApiClient client;
        private static string repo;
        private static long lastCreatedIssueNumber;
        private static long lastCreatedCommentId;


        [SetUp]
        public void Setup()
        {            
            //client = new GitHubApiClient("https://api.github.com/repos/testnakov/", "your_username", "token");
            client = new GitHubApiClient("https://api.github.com/repos/qatestst/", "qatestst", "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");
            repo = "RestSharp-HTTP-Post-Test";
        }


        [Test, Order (1)]
        public void Test_GetAllIssuesFromARepo()
        {
            //Arrange


            //Act
            var issues = client.GetAllIssues(repo);

            //Assert
            Assert.That(issues, Has.Count.GreaterThan(0), "There should be more than one issue");
            foreach (Issue issue in issues) 
            {
                Assert.That(issue.Id, Is.GreaterThan(0), "Issue ID should be greater than 0.");
                Assert.That(issue.Number, Is.GreaterThan(0), "Issue Number should be greater than 0.");
                Assert.That(issue.Title, Is.Not.Empty, "Issue Title should not be empty");

            }


        }

        [Test, Order (2)]
        public void Test_GetIssueByValidNumber()
        {
            // Arrange
            int issueNumber = 1;

            //Act
            var issue = client.GetIssueByNumber(repo, issueNumber);

            //Assert
            Assert.That(issue, Is.Not.Null);
            Assert.That(issue.Id, Is.GreaterThan(0), "Issue ID should be greater than 0.");
            Assert.That(issue.Number, Is.EqualTo(issueNumber), "Issue Number should be greater than 0.");
            Assert.That(issue.Title, Is.Not.Empty, "Issue Title should not be empty");



        }

        [Test, Order (3)]
        public void Test_GetAllLabelsForIssue()
        {
            //Arrange
            int issueNumber = 4;

            //Act
            var labels = client.GetAllLabelsForIssue(repo, issueNumber);

            //Assert
            Assert.That(labels.Count, Is.GreaterThan(0), "There should be labels on the issue");
            foreach (var label in labels)
            {
                Assert.That(label.Id, Is.GreaterThan(0), "Label ID should be more than 0");
                Assert.That(label.Name, Is.Not.Null, "Label name should not be null");
                
                Console.WriteLine($"Label: {label.Id} - Name: {label.Name}");
            }

        }

        [Test, Order (4)]
        public void Test_GetAllCommentsForIssue()
        {
            //Arrange
            int issueNumber = 4;

            //Act
            var comments = client.GetAllCommentsForIssue(repo, issueNumber);

            //Assert
            Assert.That(comments.Count, Is.GreaterThan(0), "There should be comments on the issue");
            foreach (var comment in comments)
            {
                Assert.That(comment.Id, Is.GreaterThan(0), "Comment ID should be more than 0");
                Assert.That(comment.Body, Is.Not.Null, "Comment body should not be null");

                Console.WriteLine($"Comment ID: {comment.Id} - Body: {comment.Body}");
            }

        }

        [Test, Order(5)]
        public void Test_CreateGitHubIssue()
        {
            //Arrange
            string title = "Hello New issue for testing";
            string body = "Added Some text here for testing";

            //Act
            var issue = client.CreateIssue(repo, title, body);

            //Assert
            //Assert.That(issue, Is.Not.Null);
            //Assert.That(issue.Title, Is.EqualTo(title));
            //Assert.That(issue.Body, Is.EqualTo(body));
            Assert.Multiple(() =>
            {
                Assert.That(issue.Id, Is.GreaterThan(0));
                Assert.That(issue.Number, Is.GreaterThan(0));
                Assert.That(issue.Title, Is.Not.Empty);
                Assert.That(issue.Title, Is.EqualTo(title));
                Assert.That(issue.Body, Is.Not.Null);
                Assert.That(issue.Body, Is.EqualTo(body));
            });

            Console.WriteLine(issue.Number);
            lastCreatedIssueNumber = issue.Number;


        }

        [Test, Order (6)]
        public void Test_CreateCommentOnGitHubIssue()
        {
            //Arrange
            long issueNumber = lastCreatedIssueNumber;
            string body = "Test Added Some text here for testing";
            
            //Act
            var comment = client.CreateCommentOnGitHubIssue(repo, issueNumber, body);
            
            //Assert
            Assert.That(comment.Body, Is.EqualTo(body));
            Console.WriteLine(comment.Id);
            lastCreatedCommentId = comment.Id;


        }

        [Test, Order (7)]
        public void Test_GetCommentById()
        {
            //Arrange
            
            long commentId = lastCreatedCommentId;
            string expectedBody = "Test Added Some text here for testing";
            //Act
            var comment = client.GetCommentById(repo, commentId);

            //Assert
            Assert.IsNotNull(comment, "Expected to retrieve a comment, but got null."); 
            Assert.That(comment.Body, Is.Not.Empty);
            Assert.That(comment.Id, Is.EqualTo(commentId));
            Assert.That(comment.Body, Is.EqualTo(expectedBody));
                      
            
        }


        [Test, Order (8)]
        public void Test_EditCommentOnGitHubIssue()
        {
            //Arrange
            long commentId = lastCreatedCommentId;
            string newBody = "Edited Test Added Some text here for testing";
            
            //Act
            var comment = client.EditCommentOnGitHubIssue(repo, commentId, newBody);

            //Assert
            Assert.IsNotNull(comment, "Expected to retrieve a comment, but got null.");
            Assert.That(comment.Body, Is.Not.Empty);
            Assert.That(comment.Id, Is.EqualTo(commentId));
            Assert.That(comment.Body, Is.EqualTo(newBody));

        }

        [Test, Order (9)]
        public void Test_DeleteCommentOnGitHubIssue()
        {
            //Arrange
            long commentid = lastCreatedCommentId;
            
            //Act
            bool result = client.DeleteCommentOnGitHubIssue(repo, commentid);

            //Assert
            Assert.IsTrue(result, "The comment should be successfully deleted.");
            

        }


    }
}

