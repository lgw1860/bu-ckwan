package guimess;
import javax.swing.JFrame;
public class LabelTest {

	public static void main(String args[])
	{
		LabelFrame labelFrame = new LabelFrame();
		labelFrame.setVisible(true);
		labelFrame.setSize(300,200);
		labelFrame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		
		int i = 0;
		while(true)
		{
			System.out.println(i);
			i++;
		}
	}
}
