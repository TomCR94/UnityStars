using System;
using System.Collections.Generic;
using GameSparks.Core;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;

//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!
//THIS FILE IS AUTO GENERATED, DO NOT MODIFY!!

namespace GameSparks.Api.Requests{
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
		
		public LogEventRequest_GSD Set_cR2( string value )
		{
			request.AddString("cR2", value);
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
		public LogChallengeEventRequest_GSD Set_cR2( string value )
		{
			request.AddString("cR2", value);
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
	
}
	

namespace GameSparks.Api.Messages {


}
