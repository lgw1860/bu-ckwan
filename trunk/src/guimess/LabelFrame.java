package guimess;

import java.awt.FlowLayout;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.Icon;
import javax.swing.ImageIcon;
import javax.swing.SwingConstants;

public class LabelFrame extends JFrame
{
	private JLabel label1; //JLabel with just text
	private JLabel label2; //with text and icon
	private JLabel label3; //label with added text and icon
	
	public LabelFrame()
	{
		super("Testing JLabel this is an awesome title");
		setLayout(new FlowLayout());
		
		//JLabel constructor with just a string argument
		label1 = new JLabel("Label with only text");
		label1.setToolTipText("Wow a tooltip for label1!");
		add(label1);
		
		//JLabel constructor with string, icon and alignment
		Icon pic = new ImageIcon(getClass().getResource("heart.jpg"));
		label2 = new JLabel("Label with text and icon!!!",pic,SwingConstants.LEFT);
		label2.setToolTipText("Wow tooltip for image!");
		add(label2);
		
		//JLabel constructor no arguments
		label3 = new JLabel();
		label3.setText("Label with icon and text at bottom - I set this!");
		label3.setIcon(pic);
		label3.setHorizontalTextPosition(SwingConstants.CENTER);
		label3.setVerticalTextPosition(SwingConstants.BOTTOM);
		label3.setToolTipText("hey yo tool tip for icon 3 in da house");
		add(label3);
	}
	
}
