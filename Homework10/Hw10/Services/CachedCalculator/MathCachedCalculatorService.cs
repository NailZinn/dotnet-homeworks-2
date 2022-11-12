using Hw10.DbModels;
using Hw10.Dto;
using Microsoft.EntityFrameworkCore;

namespace Hw10.Services.CachedCalculator;

public class MathCachedCalculatorService : IMathCalculatorService
{
	private readonly ApplicationContext _dbContext;
	private readonly IMathCalculatorService _simpleCalculator;

	public MathCachedCalculatorService(ApplicationContext dbContext, IMathCalculatorService simpleCalculator)
	{
		_dbContext = dbContext;
		_simpleCalculator = simpleCalculator;
	}

	public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        var result = await _dbContext.SolvingExpressions
            .FirstOrDefaultAsync(exp => exp.Expression == expression);

        if (result is not null)
            return new CalculationMathExpressionResultDto(result.Result);

        var dto = await _simpleCalculator.CalculateMathExpressionAsync(expression);

        if (dto.IsSuccess)
        {
            _dbContext.SolvingExpressions.Add(new SolvingExpression
            {
                Expression = expression,
                Result = dto.Result
            });

            await _dbContext.SaveChangesAsync();
        }

        return dto;
	}
}