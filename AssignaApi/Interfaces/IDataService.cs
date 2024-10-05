using System.Collections.Generic;
using System.Threading.Tasks;
using AssignaApi.Models;
using AssignaApi.Response;

namespace AssignaApi.Interfaces
{
    public interface IDataService
    {
        // user register
        Task<AuthResult> UserRegisterAsync(UserRegister data);

        // get all users
        List<UsersDto> AllUsers();

        // store forgot password token
        Task<AuthResult> ForgotTokenAsync(ForgotPassword data);

        // reset password
        Task<Result> ResetPasswordAsync(ResetPassword data);

        // reset verify token and refresh token
        Task<AuthResult> ResetVerifyTokenAsync(RefreshToken data);

        #region team lead tasks

        // get team members
        List<UsersDto> TeamMembers();

        // get categories
        List<CategoryDto> AllCategories();

        // get priorities
        List<PriorityDto> Priorities();

        // add a new task
        Task<Result> SaveTaskAsync(TaskDto data);

        // edit a task
        Task<Result> EditTaskAsync(TaskDto data);

        // delete a task
        Task<Result> DeleteTaskAsync(DeleteTask data);

        #endregion

        // all tasks
        Task<List<TaskDto>> AllTasks();

        // pending taks
        Task<List<TaskDto>> Pendings();

        // completed tasks
        Task<List<TaskDto>> Completed();

        // high priority tasks
        Task<List<TaskDto>> HighPriority();

        // medium priority tasks
        Task<List<TaskDto>> MediumPriority();

        // low priority tasks
        Task<List<TaskDto>> LowPriority();

        // task infomation
        Task<List<TaskDto>> TaskInfo(int taskId);

        #region team member

        // add task note
        Task<Result> AddTaskNoteAsync(TaskDto data);

        // mark as done
        Task<Result> MarkasDoneAsync(MarkDone data);

        #endregion

        // get google user infomation
        Task<GoogleResponse> GoogleUserInfomation(string accessToken);

        // external signin
        AuthResult ExternalSignIn(string email);

        // external signup
        Task<AuthResult> ExternalSignUp(ExternalSignUp data);

    }
}