using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProjectService
{
    private readonly GenericRepository<Project> _projectRepository;

    public ProjectService(GenericRepository<Project> projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<IEnumerable<Project>> GetAllProjectsAsync()
    {
        return await _projectRepository.GetAllAsync();
    }

    public async Task<Project> GetProjectByIdAsync(int id)
    {
        return await _projectRepository.GetByIdAsync(id);
    }

    public async Task AddProjectAsync(Project project)
    {
        await _projectRepository.AddAsync(project);
    }

    public async Task UpdateProjectAsync(Project project)
    {
        await _projectRepository.UpdateAsync(project);
    }

    public async Task DeleteProjectAsync(int id)
    {
        var project = await _projectRepository.GetByIdAsync(id);
        if (project != null)
        {
            await _projectRepository.DeleteAsync(project);
        }
    }
}
