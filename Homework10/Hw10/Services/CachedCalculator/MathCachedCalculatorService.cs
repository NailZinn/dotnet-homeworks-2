using Hw10.Dto;
using Microsoft.Extensions.Caching.Memory;

namespace Hw10.Services.CachedCalculator;

public class MathCachedCalculatorService : IMathCalculatorService
{
	private readonly IMemoryCache _cache;
	private readonly IMathCalculatorService _simpleCalculator;

	public MathCachedCalculatorService(IMemoryCache cache, IMathCalculatorService simpleCalculator)
	{
		_cache = cache;
		_simpleCalculator = simpleCalculator;
	}

	public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
	{
		if (expression is not null)
		{
			var result = _cache.Get<double?>(expression);

			if (result is not null)
				return new CalculationMathExpressionResultDto(result.Value);
		}

        var dto = await _simpleCalculator.CalculateMathExpressionAsync(expression);

        if (dto.IsSuccess)
        {
	        _cache.Set(expression, dto.Result);
        }

        return dto;
	}
}