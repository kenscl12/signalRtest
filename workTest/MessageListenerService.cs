using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace workTest
{
	/// <summary>
	/// Сервис отправляющий сообщения
	/// </summary>
	public class MessageListenerService : IHostedService, IDisposable
	{
		/// <summary>
		/// Таймер
		/// </summary>
		private Timer _timer;

		/// <summary>
		/// MessageHub
		/// </summary>
		private IHubContext<MessageHub> _messageHub;

		/// <summary>
		/// Сообщения
		/// </summary>
		private List<string> messages = new List<string>();

		/// <summary>
		/// Новое сообщение
		/// </summary>
		private string newMessage = "newMessage";

		public MessageListenerService(IHubContext<MessageHub> messageHub)
		{
			_messageHub = messageHub;
		}

		/// <summary>
		/// Определяет отправку сообщений
		/// </summary>
		/// <param name="cancellationToken">CancellationToken</param>
		public Task StartAsync(CancellationToken cancellationToken)
		{
			_timer = new Timer(SendMessage, null, TimeSpan.Zero,
				TimeSpan.FromSeconds(1));

			return Task.CompletedTask;
		}

		/// <summary>
		/// Отправляет сообщения
		/// </summary>
		/// <param name="state">Состояние</param>
		private void SendMessage(object state)
		{
			messages.Add(newMessage);
			_messageHub.Clients.All.SendAsync("Send", messages);
		}

		/// <summary>
		/// Останавливает отправку
		/// </summary>
		/// <param name="cancellationToken">CancellationToken</param>
		public Task StopAsync(CancellationToken cancellationToken)
		{
			_timer?.Change(Timeout.Infinite, 0);

			return Task.CompletedTask;
		}

		#region IDisposable Support

		public void Dispose()
		{
			_timer?.Dispose();
		}

		#endregion
	}
}
