using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TeamsAllocationManager.Contracts.Base.Commands;
using TeamsAllocationManager.Contracts.Desks.Commands;
using TeamsAllocationManager.Database.Repositories;
using TeamsAllocationManager.Database.Repositories.Interfaces;

namespace TeamsAllocationManager.Infrastructure.Handlers.Desk;

public class ToggleDeskIsEnabledHandler : IAsyncCommandHandler<ToggleDeskIsEnabledCommand>
{
	private readonly IDesksRepository _desksRepository;

	public ToggleDeskIsEnabledHandler(IDesksRepository desksRepository)
	{
		_desksRepository = desksRepository;
	}

	public async Task HandleAsync(ToggleDeskIsEnabledCommand command, CancellationToken cancellationToken = default)
	{
		var desksToToggleEnable = await _desksRepository.GetDesks(command.DesksIdsToToggleEnable);

		desksToToggleEnable.ToList().ForEach(desk =>
		{
			desk.ToggleIsEnabled();
		});

		await _desksRepository.UpdateRangeAsync(desksToToggleEnable);
	}
}
