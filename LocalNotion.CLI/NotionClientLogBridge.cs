// Copyright (c) Herman Schoenfeld 2018 - Present. All rights reserved. (https://sphere10.com/products/localnotion)
// Author: Herman Schoenfeld <herman@sphere10.com>
//
// Distributed under the GPLv3 software license, see the accompanying file LICENSE
// or visit https://github.com/HermanSchoenfeld/localnotion/blob/master/LICENSE
//
// This notice must not be removed when duplicating this file or its contents, in whole or in part.

using MEL = Microsoft.Extensions.Logging;

namespace LocalNotion.CLI;

/// <summary>
/// Forwards Notion.Client's warnings and errors (e.g. request retries) to the CLI logger.
/// Only the formatted message is written, never the exception object; lower levels are dropped.
/// </summary>
internal sealed class NotionClientLogBridge : MEL.ILoggerFactory, MEL.ILogger {
	private readonly Sphere10.Framework.ILogger _logger;

	public NotionClientLogBridge(Sphere10.Framework.ILogger logger) {
		_logger = logger;
	}

	public MEL.ILogger CreateLogger(string categoryName) => this;

	public void AddProvider(MEL.ILoggerProvider provider) {
	}

	public void Dispose() {
	}

	public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

	public bool IsEnabled(MEL.LogLevel logLevel) => logLevel >= MEL.LogLevel.Warning && logLevel != MEL.LogLevel.None;

	public void Log<TState>(MEL.LogLevel logLevel, MEL.EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) {
		if (!IsEnabled(logLevel))
			return;
		var message = formatter(state, exception);
		if (logLevel == MEL.LogLevel.Warning)
			_logger.Warning(message);
		else
			_logger.Error(message);
	}
}
