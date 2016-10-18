using System;
using System.Diagnostics.CodeAnalysis;
using System.Reactive;
using GitHub.Models;
using GitHub.ViewModels;
using ReactiveUI;

namespace GitHub.SampleData
{
    [ExcludeFromCodeCoverage]
    public class PullRequestDetailViewModelDesigner : BaseViewModel, IPullRequestDetailViewModel
    {
        public PullRequestDetailViewModelDesigner()
        {
            Model = new PullRequestModel(419, 
                "Error handling/bubbling from viewmodels to views to viewhosts",
                 new AccountDesigner { Login = "shana", IsUser = true },
                 DateTime.Now.Subtract(TimeSpan.FromDays(3)))
            {
                State = PullRequestStateEnum.Open,
                CommitCount = 9,
            };

            SourceBranchDisplayName = "shana/error-handling";
            TargetBranchDisplayName = "master";
            Body = @"Adds a way to surface errors from the view model to the view so that view hosts can get to them.

ViewModels are responsible for handling the UI on the view they control, but they shouldn't be handling UI for things outside of the view. In this case, we're showing errors in VS outside the view, and that should be handled by the section that is hosting the view.

This requires that errors be propagated from the viewmodel to the view and from there to the host via the IView interface, since hosts don't usually know what they're hosting.

![An image](https://cloud.githubusercontent.com/assets/1174461/18882991/5dd35648-8496-11e6-8735-82c3a182e8b4.png)";

            var gitHubDir = new PullRequestDirectoryViewModel("GitHub");
            var modelsDir = new PullRequestDirectoryViewModel("Models");
            var repositoriesDir = new PullRequestDirectoryViewModel("Repositories");
            var itrackingBranch = new PullRequestFileViewModel(@"GitHub\Models\ITrackingBranch.cs", PullRequestFileStatus.Modified);
            var oldBranchModel = new PullRequestFileViewModel(@"GitHub\Models\OldBranchModel.cs", PullRequestFileStatus.Removed);
            var concurrentRepositoryConnection = new PullRequestFileViewModel(@"GitHub\Repositories\ConcurrentRepositoryConnection.cs", PullRequestFileStatus.Added);

            repositoriesDir.Files.Add(concurrentRepositoryConnection);
            modelsDir.Directories.Add(repositoriesDir);
            modelsDir.Files.Add(itrackingBranch);
            modelsDir.Files.Add(oldBranchModel);
            gitHubDir.Directories.Add(modelsDir);

            ChangedFilesTree = new ReactiveList<IPullRequestChangeNode>();
            ChangedFilesTree.Add(gitHubDir);

            ChangedFilesList = new ReactiveList<IPullRequestFileViewModel>();
            ChangedFilesList.Add(concurrentRepositoryConnection);
            ChangedFilesList.Add(itrackingBranch);
            ChangedFilesList.Add(oldBranchModel);

            CheckoutMode = CheckoutMode.Fetch;
        }

        public IPullRequestModel Model { get; }
        public string SourceBranchDisplayName { get; }
        public string TargetBranchDisplayName { get; }
        public string Body { get; }
        public ChangedFilesViewType ChangedFilesViewType { get; set; }
        public OpenChangedFileAction OpenChangedFileAction { get; set; }
        public IReactiveList<IPullRequestChangeNode> ChangedFilesTree { get; }
        public IReactiveList<IPullRequestFileViewModel> ChangedFilesList { get; }
        public CheckoutMode CheckoutMode { get; set; }
        public string CheckoutError { get; set; }
        public int CommitsBehind { get; set; }
        public string CheckoutDisabledMessage { get; set; }

        public ReactiveCommand<Unit> Checkout { get; }
        public ReactiveCommand<object> OpenOnGitHub { get; }
        public ReactiveCommand<object> ActivateItem { get; }
        public ReactiveCommand<object> ToggleChangedFilesView { get; }
        public ReactiveCommand<object> ToggleOpenChangedFileAction { get; }
        public ReactiveCommand<object> OpenFile { get; }
        public ReactiveCommand<object> DiffFile { get; }
    }
}