using System;
using System.Collections.Generic;
using GameSparks.Core;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;

//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!

namespace GameSparks.Api.Requests{
	public class LogEventRequest_DeleteUploaded : GSTypedRequest<LogEventRequest_DeleteUploaded, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_DeleteUploaded() : base("LogEventRequest"){
			request.AddString("eventKey", "DeleteUploaded");
		}
		
		public LogEventRequest_DeleteUploaded Set_Upload( string value )
		{
			request.AddString("Upload", value);
			return this;
		}
	}
	
	public class LogChallengeEventRequest_DeleteUploaded : GSTypedRequest<LogChallengeEventRequest_DeleteUploaded, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_DeleteUploaded() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "DeleteUploaded");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_DeleteUploaded SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_DeleteUploaded Set_Upload( string value )
		{
			request.AddString("Upload", value);
			return this;
		}
	}
	
	public class LogEventRequest_GSD : GSTypedRequest<LogEventRequest_GSD, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_GSD() : base("LogEventRequest"){
			request.AddString("eventKey", "GSD");
		}
		
		public LogEventRequest_GSD Set_s( string value )
		{
			request.AddString("s", value);
			return this;
		}
		
		public LogEventRequest_GSD Set_d( string value )
		{
			request.AddString("d", value);
			return this;
		}
		
		public LogEventRequest_GSD Set_cR1( string value )
		{
			request.AddString("cR1", value);
			return this;
		}
		
		public LogEventRequest_GSD Set_challengeInstanceId( string value )
		{
			request.AddString("challengeInstanceId", value);
			return this;
		}
	}
	
	public class LogChallengeEventRequest_GSD : GSTypedRequest<LogChallengeEventRequest_GSD, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_GSD() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "GSD");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_GSD SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_GSD Set_s( string value )
		{
			request.AddString("s", value);
			return this;
		}
		public LogChallengeEventRequest_GSD Set_d( string value )
		{
			request.AddString("d", value);
			return this;
		}
		public LogChallengeEventRequest_GSD Set_cR1( string value )
		{
			request.AddString("cR1", value);
			return this;
		}
		public LogChallengeEventRequest_GSD Set_challengeInstanceId( string value )
		{
			request.AddString("challengeInstanceId", value);
			return this;
		}
	}
	
	public class LogEventRequest_getOnlinePlayers : GSTypedRequest<LogEventRequest_getOnlinePlayers, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_getOnlinePlayers() : base("LogEventRequest"){
			request.AddString("eventKey", "getOnlinePlayers");
		}
	}
	
	public class LogChallengeEventRequest_getOnlinePlayers : GSTypedRequest<LogChallengeEventRequest_getOnlinePlayers, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_getOnlinePlayers() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "getOnlinePlayers");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_getOnlinePlayers SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
	}
	
	public class LogEventRequest_IRD : GSTypedRequest<LogEventRequest_IRD, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_IRD() : base("LogEventRequest"){
			request.AddString("eventKey", "IRD");
		}
		
		public LogEventRequest_IRD Set_cR2( string value )
		{
			request.AddString("cR2", value);
			return this;
		}
		
		public LogEventRequest_IRD Set_challengeInstanceId( string value )
		{
			request.AddString("challengeInstanceId", value);
			return this;
		}
	}
	
	public class LogChallengeEventRequest_IRD : GSTypedRequest<LogChallengeEventRequest_IRD, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_IRD() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "IRD");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_IRD SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_IRD Set_cR2( string value )
		{
			request.AddString("cR2", value);
			return this;
		}
		public LogChallengeEventRequest_IRD Set_challengeInstanceId( string value )
		{
			request.AddString("challengeInstanceId", value);
			return this;
		}
	}
	
	public class LogEventRequest_TT : GSTypedRequest<LogEventRequest_TT, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_TT() : base("LogEventRequest"){
			request.AddString("eventKey", "TT");
		}
		
		public LogEventRequest_TT Set_GD( string value )
		{
			request.AddString("GD", value);
			return this;
		}
	}
	
	public class LogChallengeEventRequest_TT : GSTypedRequest<LogChallengeEventRequest_TT, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_TT() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "TT");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_TT SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
		public LogChallengeEventRequest_TT Set_GD( string value )
		{
			request.AddString("GD", value);
			return this;
		}
	}
	
	public class LogEventRequest_Win : GSTypedRequest<LogEventRequest_Win, LogEventResponse>
	{
	
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogEventResponse (response);
		}
		
		public LogEventRequest_Win() : base("LogEventRequest"){
			request.AddString("eventKey", "Win");
		}
	}
	
	public class LogChallengeEventRequest_Win : GSTypedRequest<LogChallengeEventRequest_Win, LogChallengeEventResponse>
	{
		public LogChallengeEventRequest_Win() : base("LogChallengeEventRequest"){
			request.AddString("eventKey", "Win");
		}
		
		protected override GSTypedResponse BuildResponse (GSObject response){
			return new LogChallengeEventResponse (response);
		}
		
		/// <summary>
		/// The challenge ID instance to target
		/// </summary>
		public LogChallengeEventRequest_Win SetChallengeInstanceId( String challengeInstanceId )
		{
			request.AddString("challengeInstanceId", challengeInstanceId);
			return this;
		}
	}
	
}
	

namespace GameSparks.Api.Messages {


}
