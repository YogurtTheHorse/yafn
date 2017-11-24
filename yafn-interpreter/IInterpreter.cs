namespace Yafn.Interpreter {
	public interface IInterpreter {
		int Cursor { get; }
		byte[] ByteCode { get; }

		void Pause();
		void Step();
		void StepOver();
	}
}
