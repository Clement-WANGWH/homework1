package ui;

import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.border.EmptyBorder;
import javax.swing.JTextField;
import javax.swing.JButton;
import javax.swing.JLabel;
import java.awt.event.ActionListener;
import java.awt.event.ActionEvent;

public class Stopwatch_JFrame {
	public static void main(String[] args) {
		new window("20216589 王文汗");
	}
}

	/**
	 * Create the frame.
	 */
	class window extends JFrame{
		private JPanel contentPane;
		private JTextField texttime;
		boolean flag = false;
		double count = 0;
		
		window(String name){
		setTitle(name);
		setBounds(100, 100, 356, 94);
		contentPane = new JPanel();
		contentPane.setBorder(new EmptyBorder(5, 5, 5, 5));
		setContentPane(contentPane);
		contentPane.setLayout(null);
		
		texttime = new JTextField();
		texttime.setText("0.00");
		texttime.setEditable(false);
		texttime.setBounds(24, 10, 83, 39);
		contentPane.add(texttime);
		texttime.setColumns(10);
		
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		this.setVisible(true);
		
		JButton btnpause = new JButton("\u5F00\u59CB");
		btnpause.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				flag=!flag;
				btnpause.setText(flag==true?"暂停":"开始");
			}
		});
		btnpause.setBounds(133, 18, 93, 23);
		contentPane.add(btnpause);
		
		JButton btnclear = new JButton("\u6E05\u96F6");
		btnclear.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				btnpause.setText("开始");
				flag=false;
				count=0;
			}
		});
		btnclear.setBounds(241, 18, 93, 23);
		contentPane.add(btnclear);
		
		JLabel lblNewLabel = new JLabel("\u79D2");
		lblNewLabel.setBounds(109, 10, 21, 39);
		contentPane.add(lblNewLabel);
		
		while(true) {
			texttime.setText(String.valueOf(count/100));
			if(flag) {
				count++;
				try {
					Thread.sleep(10);
				} catch (InterruptedException e1) {
					// TODO 自动生成的 catch 块
					e1.printStackTrace();
				}
			}
			
		}
	}
		}
